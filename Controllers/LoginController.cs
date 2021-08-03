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
using Newtonsoft.Json.Linq;

namespace Trello.API.Controllers
{
	[EnableCors("Policy1")]
	[ApiController]
	[Route("[controller]")]

	public class LoginController : ControllerBase
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<LoginController> _log;
		private readonly IConfiguration _config;

		public LoginController(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<LoginController>();
			_config = config;
		}

		/// <summary>
		/// Faz login.
		/// </summary>
		/// <returns>ID do Userr caso 200OK</returns>
		/// <remarks>
		/// <para>
		///		
		/// </para>
		///	<![CDATA[
		/// Acesse : http://dontpad.com/examplepostuser e colete o exemplo
		/// ]]>
		///	
		/// <para>
		/// </para>
		/// 
		/// </remarks>

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Post([FromBody] string loginParam)
		{

			LoginBO loginBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Post('{JsonConvert.SerializeObject(loginParam, Formatting.None)}')");

				loginBO = new LoginBO(_loggerFactory, _config);

				dynamic login = JObject.Parse(loginParam);

				login = loginBO.Validade(login);

				response = Ok(login);

				_log.LogInformation($"Finishing Post");
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
