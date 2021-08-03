using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Trello.API.Data.Repository;
using Trello.API.Model;

namespace Trello.API.Business
{
    public class ListBO
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ListBO> _log;
        private readonly IConfiguration _config;

        public ListBO(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _loggerFactory = loggerFactory;
            _log = loggerFactory.CreateLogger<ListBO>();
            _config = config;
        }

		#region Change Data

		public List Insert(List list)
		{
			ListRepository ListRepository;

			try
			{
				ListRepository = new ListRepository(_loggerFactory, _config);

				if (list.ID == 0)
				{

					list = ListRepository.Insert(list);
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

			return list;
		}

		public List Update(List list)
		{
			ListRepository listRepository;

			try
			{
				listRepository = new ListRepository(_loggerFactory, _config);

				if (list.ID == 0)
				{
					throw new Exception("ID diferente de 0, avalie a utilização do POST");
				}
				else
				{

					listRepository.Update(list);
				}

			}
			catch (Exception exception)
			{
				throw exception;
			}

			return list;
		}
		public void Delete(int id)
		{
			ListRepository listRepository;
			List list;

			try
			{
				if (id == 0)
				{
					throw new Exception("ID inválido");
				}
				else
				{
					listRepository = new ListRepository(_loggerFactory, _config);

					list = Get(id);
					if (list != null)
					{

						listRepository.Delete(id);
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

        public List Get(int id)
		{
			ListRepository listRepository;
			List list;

			try
			{
				listRepository = new ListRepository(_loggerFactory, _config);
				list = new List();

				list = listRepository.Get(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return list;
		}


		public List<List> Get()
		{
			ListRepository ListRepository;
			List<List> lists;

			try
			{
				ListRepository = new ListRepository(_loggerFactory, _config);
				lists = new List<List>();

				lists = ListRepository.Get();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return lists;
		}

        #endregion
    }

}
