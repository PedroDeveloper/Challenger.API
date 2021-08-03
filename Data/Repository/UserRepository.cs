using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Trello.API.Data.Base;
using Trello.API.Model;


namespace Trello.API.Data.Repository
{
	public class UserRepository
	{

		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<UserRepository> _log;
		private readonly IConfiguration _config;

		public UserRepository(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<UserRepository>();
			_config = config;
		}

		#region LoadModel


		private List <User> Load(DataSet data)
		{
			List<User> users;
			User user;
			try
			{
				users = new List <User>();
				foreach (DataRow row in data.Tables[0].Rows)
				{
					user = new User();
					user.ID = row.Field<int>("ID");
					user.Name = row.Field<string>("Name");
					user.Email= row.Field<string>("Email");
					users.Add(user);
				}

				return users;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			
		}

		#endregion


		#region Change Data

		public User Insert(User user)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"INSERT INTO Users
											(
												 Name
												,Email
												,Password
											)
										 OUTPUT inserted.ID 
										 VALUES
											(
												 @Name
												,@Email
												,@Password
											)");

				command.Parameters.AddWithValue("Name", user.Name.AsDbValue());
				command.Parameters.AddWithValue("Email", user.Email.AsDbValue());
				command.Parameters.AddWithValue("Password", user.Password.AsDbValue());

				user.ID = (int)dataConnection.ExecuteScalar(command);
			}
			catch (Exception exception)
			{
				throw exception;
			}

			return user;
		}

		public User Update(User user)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"UPDATE Users SET

											 Name = @Name
											,Email = @Email
											,Password = @Password

											WHERE ID = @ID");

				command.Parameters.AddWithValue("ID", user.ID.AsDbValue());
				command.Parameters.AddWithValue("Name", user.Name.AsDbValue());
				command.Parameters.AddWithValue("Email", user.Email.AsDbValue());
				command.Parameters.AddWithValue("Password", user.Password.AsDbValue());

				dataConnection.ExecuteNonQuery(command);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return user;
		}

		public bool Delete(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			int result;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"DELETE from Users WHERE ID = @ID");

				command.Parameters.AddWithValue("ID", id.AsDbValue());

				result = dataConnection.ExecuteNonQuery(command);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return (result > 0);
		}

		#endregion

		#region Retrieve Data

		public int GetUserPassword(dynamic login)
		{
			int id;
			string userName = login["userName"];
			string userPassword = login["userPassword"];

			SqlHelper dataConnection;
			DataSet dataSet;
			SqlCommand command;

            try
            {
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"select u.ID from Users u where u.Email = @UserName and u.Password = @userPassword ");
                command.Parameters.AddWithValue("UserName", userName.AsDbValue());
                command.Parameters.AddWithValue("userPassword", userPassword.AsDbValue());
                id = Convert.ToInt32(dataConnection.ExecuteScalar(command));

				return id;

			}
			catch (Exception exception)
			{

				throw exception;
			}
		}

		public List<User> Get()
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			List<User> users;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM Users");
				dataSet = dataConnection.ExecuteDataSet(command);

				users = Load(dataSet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return users;
		}
		public User Get(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			User user;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM Users WHERE ID = @ID");
				command.Parameters.AddWithValue("ID", id.AsDbValue());
				dataSet = dataConnection.ExecuteDataSet(command);
				user = Load(dataSet).FirstOrDefault();
			}
			catch (Exception exception)
			{
				throw exception;
			}
			return user;
		}


		#endregion
	}

}

