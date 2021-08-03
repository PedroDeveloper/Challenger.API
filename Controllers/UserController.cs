using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Trello.API.Business;
using Trello.API.Model;

namespace Trello.API.Controllers
{
	[EnableCors("Policy1")]
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<UserController> _log;
		private readonly IConfiguration _config;

		public UserController(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<UserController>();
			_config = config;
		}

		/// <summary>
		/// Retornar Usuários.
		/// </summary>
		/// <returns>retorna dados dos usários</returns>

		[HttpGet]
		[ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get()
		{
			UserBO userBO;
			List<User> Users;
			ObjectResult response;

			try
			{
				_log.LogInformation("Starting Get()");
				Users = new List<User>();

				userBO = new UserBO(_loggerFactory, _config);
				Users = userBO.Get();

				response = Ok(Users);

				_log.LogInformation($"Finishing Get() with '{Users.Count}' results");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}

		/// <summary>
		/// Retornar dados de um User existente.
		/// </summary>
		/// <returns>retorna dados do usário cujo ID </returns>

		[HttpGet("{id}", Name = "GetUser")]
		[ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get(int id)
		{
			UserBO userBO;
			User user;
			ObjectResult response;

			try
			{
				user = new User();
				_log.LogInformation($"Starting Get( {id} )");

				userBO = new UserBO(_loggerFactory, _config);

				user = userBO.Get(id);

				if (user != null)
				{
					response = Ok(user);
				}
				else
				{
					response = NotFound(string.Empty);
				}

				_log.LogInformation($"Finishing Get( {id} )");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}

		/// <summary>
		/// Cadatrar usuário.
		/// </summary>
		/// <returns>retorna dados do usário cadastrado</returns>

		[HttpPost]
		[ProducesResponseType(typeof(User),StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Post([FromBody] User user)
		{
			UserBO userBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Post('{JsonConvert.SerializeObject(user, Formatting.None)}')");

				userBO = new UserBO(_loggerFactory, _config);

				user = userBO.Insert(user);

				response = Ok(user);

				_log.LogInformation($"Finishing Post");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}

		/// <summary>
		/// Editar Cadastro de usuário.
		/// </summary>
		/// <returns>retorna dados do usário atualualizados</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Put(int id, User user)
		{
			UserBO userBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Put( {id}, '{JsonConvert.SerializeObject(user, Formatting.None)}')");

				userBO = new UserBO(_loggerFactory, _config);

				user.ID = id;
				user = userBO.Update(user);

				response = Ok(user);

				_log.LogInformation($"Finishing Put( {id} )");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}

		/// <summary>
		/// Excluir usário.
		/// </summary>
		/// <returns></returns>
		/// 
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			UserBO userBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Delete( {id} )");

				userBO = new UserBO(_loggerFactory, _config);
				userBO.Delete(id);

				response = Ok(string.Empty);

				_log.LogInformation($"Finishing Delete( {id} )");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}
	}
}
