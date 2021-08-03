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
	public class ListRepository
	{

		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<ListRepository> _log;
		private readonly IConfiguration _config;

		public ListRepository(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<ListRepository>();
			_config = config;
		}

		#region LoadModel


		private List <List> Load(DataSet data)
		{
			List<List> lists;
			List list;

			try
			{
				lists = new List <List>();

				foreach (DataRow row in data.Tables[0].Rows)
				{
					list = new List();

					list.ID = row.Field<int>("ID");
					list.BoardID = row.Field<int>("BoardsID");
					list.Title= row.Field<string>("Title");
					lists.Add(list);
					
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}

				return lists;

		}

		#endregion


		#region Change Data

		public List Insert(List list)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"INSERT INTO Lists
											(
												 BoardsID
												,Title
											)
										 OUTPUT inserted.ID 
										 VALUES
											(
												 @BoardsID
												,@Title
											)")
                {

                };
                command.Parameters.AddWithValue("BoardsID", list.BoardID.AsDbValue());
				command.Parameters.AddWithValue("Title", list.Title.AsDbValue());

				list.ID = (int)dataConnection.ExecuteScalar(command);
			}
			catch (Exception exception)
			{
				throw exception;
			}

			return list;
		}

		public List Update(List list)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"UPDATE Lists SET

											 BoardsID = @BoardsID
											,Title = @Title

											WHERE ID = @ID");

				command.Parameters.AddWithValue("ID", list.ID.AsDbValue());
				command.Parameters.AddWithValue("BoardsID", list.BoardID.AsDbValue());
				command.Parameters.AddWithValue("Title", list.Title.AsDbValue());

				dataConnection.ExecuteNonQuery(command);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return list;
		}

		public bool Delete(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			int result;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"DELETE from Lists WHERE ID = @ID");

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

		public List Get(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			List list;

			try
			{
				list = new List();
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM Lists WHERE ID = @id");
				command.Parameters.AddWithValue("ID", id.AsDbValue());
				dataSet = dataConnection.ExecuteDataSet(command);
				list = Load(dataSet).FirstOrDefault();
			}
			catch (Exception exception)
			{
				throw exception;
			}
			return list;
		}


		public List <List> Get()
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			List<List> lists;

			try
			{
				lists = new List<List>();
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM Lists");
				dataSet = dataConnection.ExecuteDataSet(command);
				lists = Load(dataSet);
			}
			catch (Exception exception)
			{
				throw exception;
			}
			return lists;
		}



		#endregion
	}

}

