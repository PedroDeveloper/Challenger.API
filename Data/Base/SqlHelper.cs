using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Trello.API.Data.Base
{
	public class SqlHelper
	{
		private readonly string connectionString;

		private readonly ILogger<SqlHelper> _log;
		private readonly IConfiguration _config;

		public SqlHelper(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_log = loggerFactory.CreateLogger<SqlHelper>();
			_config = config;
			connectionString = _config.GetValue("ConnectionStrings:ConnectionBase")[0];
		}


		public int ExecuteNonQuery(SqlCommand command)
		{
			int retorno;
			try
			{
				SqlConnection connection = new SqlConnection(connectionString);

				command.Connection = connection;
				command.CommandTimeout = 0;

				connection.Open();
				retorno = command.ExecuteNonQuery();

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}

		public int ExecuteNonQuery(string script, SqlParameter[] parameters = null)
		{
			int retorno;
			try
			{
				SqlConnection connection = new SqlConnection(connectionString);
				SqlCommand command = new SqlCommand();

				command.Connection = connection;
				command.CommandText = script;
				command.CommandTimeout = 0;

				if (parameters?.Count() > 0)
				{
					command.Parameters.AddRange(parameters);
					command.CommandType = CommandType.StoredProcedure;
				}
				else
				{
					command.CommandType = CommandType.Text;
				}

				connection.Open();
				retorno = command.ExecuteNonQuery();

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}


		/// <summary>
		/// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public object ExecuteScalar(SqlCommand command)
		{
			object retorno;
			try
			{
				SqlConnection connection = new SqlConnection(connectionString);

				command.Connection = connection;
				command.CommandTimeout = 0;

				connection.Open();
				retorno = command.ExecuteScalar();

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}

		/// <summary>
		/// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
		/// </summary>
		/// <param name="script"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object ExecuteScalar(string script, SqlParameter[] parameters = null)
		{
			object retorno;
			try
			{
				SqlConnection connection = new SqlConnection(connectionString);
				SqlCommand command = new SqlCommand();

				command.Connection = connection;
				command.CommandText = script;
				command.CommandTimeout = 0;

				if (parameters?.Count() > 0)
				{
					command.Parameters.AddRange(parameters);
					command.CommandType = CommandType.StoredProcedure;
				}
				else
				{
					command.CommandType = CommandType.Text;
				}

				connection.Open();
				retorno = command.ExecuteScalar();

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}


		public DataSet ExecuteDataSet(SqlCommand command)
		{
			DataSet retorno;

			try
			{
				retorno = new DataSet();

				SqlConnection connection = new SqlConnection(connectionString);

				command.Connection = connection;
				command.CommandTimeout = 0;

				connection.Open();
				SqlDataAdapter adapter = new SqlDataAdapter(command);
				adapter.Fill(retorno);

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}
		public DataSet ExecuteDataSet(string script, SqlParameter[] parameters = null)
		{
			DataSet retorno;

			try
			{
				retorno = new DataSet();

				SqlConnection connection = new SqlConnection(connectionString);
				SqlCommand command = new SqlCommand();

				command.Connection = connection;
				command.CommandText = script;
				command.CommandTimeout = 0;

				if (parameters?.Count() > 0)
				{
					command.Parameters.AddRange(parameters);
					command.CommandType = CommandType.StoredProcedure;
				}
				else
				{
					command.CommandType = CommandType.Text;
				}

				connection.Open();
				SqlDataAdapter adapter = new SqlDataAdapter(command);
				adapter.Fill(retorno);

				command.Dispose();
				connection.Dispose();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return retorno;
		}
	}
}
