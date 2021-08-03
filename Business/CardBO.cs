using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Trello.API.Data.Repository;
using Trello.API.Model;

namespace Trello.API.Business
{
    public class CardBO
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<CardBO> _log;
        private readonly IConfiguration _config;

        public CardBO(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _loggerFactory = loggerFactory;
            _log = loggerFactory.CreateLogger<CardBO>();
            _config = config;
        }

		#region Change Data

		public Card Insert(Card card)
		{
			CardRepository CardRepository;

			try
			{
				CardRepository = new CardRepository(_loggerFactory, _config);

				if (card.ID == 0)
				{

					card = CardRepository.Insert(card);
				}
				else
				{
					throw new Exception("ID diferente de 0, avalie a utilização do PUT");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return card;
		}
		public Card Update(Card card)
		{
			CardRepository cardRepository;

			try
			{
				cardRepository = new CardRepository(_loggerFactory, _config);

				if (card.ID == 0)
				{
					throw new Exception("ID diferente de 0, avalie a utilização do POST");
				}
				else
				{

					cardRepository.Update(card);
				}

			}
			catch (Exception exception)
			{
				throw exception;
			}

			return card;
		}
		public void Delete(int id)
		{
			CardRepository cardRepository;
			List <Card>cards;

			try
			{
				if (id == 0)
				{
					throw new Exception("ID inválido");
				}
				else
				{
					cardRepository = new CardRepository(_loggerFactory, _config);

					cards = Get(id);
					if (cards != null)
					{
						cardRepository.Delete(id);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		#endregion

		#region Retrieve Data

		public List<Card> Get()
		{
			CardRepository cardRepository;
			List<Card> cards;

			try
			{
				cards = new List<Card>();
				cardRepository = new CardRepository(_loggerFactory, _config);

				cards = cardRepository.Get();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return cards;
		}
		public List<Card> Get(int id)
		{
			CardRepository cardRepository;
			List<Card> cards;

			try
			{
				cardRepository = new CardRepository(_loggerFactory, _config);
				cards = new List<Card>();

				cards = cardRepository.Get(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return cards;
		}
        #endregion


    }

}
