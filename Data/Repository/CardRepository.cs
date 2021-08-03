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
	public class CardRepository
	{

		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<CardRepository> _log;
		private readonly IConfiguration _config;

		public CardRepository(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<CardRepository>();
			_config = config;
		}

		#region LoadModel


		private List <Card> Load(DataSet data)
		{
			List<Card> cards;

			Card card;
			try
			{
				cards = new List<Card>();

				foreach (DataRow row in data.Tables[0].Rows)
				{
					card = new Card();

					card.ID = row.Field<int>("ID");
					card.Title = row.Field<string>("Title");
					card.ListID =  row.Field<int>("ListsID");
					card.Description = row.Field<string>("Descripption");
					cards.Add(card);

				}

				return cards;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			
		}

		#endregion


		#region Change Data

		public Card Insert(Card card)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"INSERT INTO Cards
											(
												 ListsID
												,Title
												,descripption
											)
										 OUTPUT inserted.ID 
										 VALUES
											(
												 @ListsID
												,@Title
												,@descripption
											)")
                {

                };
                command.Parameters.AddWithValue("ListsID", card.ListID.AsDbValue());
				command.Parameters.AddWithValue("Title", card.Title.AsDbValue());
				command.Parameters.AddWithValue("descripption", card.Description.AsDbValue());

				card.ID = (int)dataConnection.ExecuteScalar(command);
			}
			catch (Exception exception)
			{
				throw exception;
			}

			return card;
		}

		public Card Update(Card card)
		{
			SqlHelper dataConnection;
			SqlCommand command;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"UPDATE Cards SET

											 ListsID = @ListsID
											,Title = @Title
											,descripption = @descripption

											WHERE ID = @ID");

				command.Parameters.AddWithValue("ListsID", card.ListID.AsDbValue());
				command.Parameters.AddWithValue("ID", card.ID.AsDbValue());
				command.Parameters.AddWithValue("Title", card.Title.AsDbValue());
				command.Parameters.AddWithValue("descripption", card.Description.AsDbValue());

				dataConnection.ExecuteNonQuery(command);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return card;
		}

		public bool Delete(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			int result;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);

				command = new SqlCommand($@"DELETE from cards WHERE ID = @ID");

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


		public List<Card> Get()
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			List<Card> cards;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM cards ");
				dataSet = dataConnection.ExecuteDataSet(command);
				cards = Load(dataSet);
			}
			catch (Exception exception)
			{
				throw exception;
			}
			return cards;
		}
		public List <Card> Get(int id)
		{
			SqlHelper dataConnection;
			SqlCommand command;
			DataSet dataSet;
			List <Card> cards;

			try
			{
				dataConnection = new SqlHelper(_loggerFactory, _config);
				command = new SqlCommand($"SELECT * FROM cards WHERE listsID =@id");
				command.Parameters.AddWithValue("id", id.AsDbValue());
				dataSet = dataConnection.ExecuteDataSet(command);
				cards = Load(dataSet);
			}
			catch (Exception exception)
			{
				throw exception;
			}
			return cards;
		}



		#endregion
	}

}

