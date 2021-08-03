using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Trello.API.Data.Repository;
using Trello.API.Model;

namespace Trello.API.Business
{
    public class LoginBO
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<LoginBO> _log;
        private readonly IConfiguration _config;

        public LoginBO(ILoggerFactory loggerFactory, IConfiguration config)
        {
            _loggerFactory = loggerFactory;
            _log = loggerFactory.CreateLogger<LoginBO>();
            _config = config;
        }


        public dynamic Validade(dynamic login)
        {
            UserRepository userRepository;
            string userName;
            string userPassword;
            User user;
            int? result;

            try
            {
                userRepository = new UserRepository(_loggerFactory, _config);
                user = new User();
                userName = login["userName"];
                userPassword = login["userPassword"];


                user.ID = userRepository.GetUserPassword(login);
                if (user.ID > 0)
                {
                    login = new { userID = user.ID, token ="tokentemp" };
                    return login;
                }
                login = new { };

            }
            catch (Exception exception)
            {
                throw exception;
            }
            return login;
           
        }
    }

}
