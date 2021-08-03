using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Trello.API.Data.Repository;
using Trello.API.Model;

namespace Trello.API.Business
{
	public class UserBO
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<UserBO> _log;
		private readonly IConfiguration _config;

		public UserBO(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<UserBO>();
			_config = config;
		}

		#region Change Data

		public User Insert(User user)
		{
			UserRepository userRepository;
			;


			try
			{
				userRepository = new UserRepository(_loggerFactory, _config);

				if (user.ID == 0)
				{

					user = userRepository.Insert(user);
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

			return user;
		}

		public User Update(User user)
		{
			UserRepository userRepository;

			try
			{
				userRepository = new UserRepository(_loggerFactory, _config);

				if (user.ID == 0)
				{
					throw new Exception("ID diferente de 0, avalie a utilização do POST");
				}
				else
				{

					userRepository.Update(user);
				}

				}
			catch (Exception exception)
			{
				throw exception;
			}

			return user;
		}

		public void Delete(int id)
		{
			UserRepository userRepository;
			User user;

			try
			{
				if (id == 0)
				{
					throw new Exception("ID inválido");
				}
				else
				{
					userRepository = new UserRepository(_loggerFactory, _config);

					user = Get(id);
					if (user != null)
					{

						userRepository.Delete(id);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		#endregion

		#region Retrieve Repository

		public User Get(int id)
		{
			UserRepository userRepository;
			User user;

			try
			{
				userRepository = new UserRepository(_loggerFactory, _config);

				user = userRepository.Get(id);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return user;
		}

		public List<User> Get()
		{
			UserRepository userRepository;
			List<User> users;

			try
			{
				userRepository = new UserRepository(_loggerFactory, _config);
				users = userRepository.Get();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return users;
		}

		#endregion
	}

	
}
