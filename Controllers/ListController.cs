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

	public class ListController : ControllerBase
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<ListController> _log;
		private readonly IConfiguration _config;

		public ListController(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<ListController>();
			_config = config;
		}

		/// <summary>
		/// Retornar Lista e cards associados.
		/// </summary>
		/// <returns>retorna dados dos usários</returns>

		[HttpGet]
		[ProducesResponseType(typeof(List), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get()
		{
			ListBO listBO;
			List<List> lists;
			ObjectResult response;

			try
			{
				_log.LogInformation("Starting Get()");
				lists = new List<List>();

				listBO = new ListBO(_loggerFactory, _config);
				lists = listBO.Get();

				response = Ok(lists);

				_log.LogInformation($"Finishing Get() with {lists.Count} results");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}




		/// <summary>
		/// Criar lista.
		/// </summary>
		/// <returns>dados da lista criada</returns>

		[HttpPost]
		[ProducesResponseType(typeof(List),StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Post([FromBody] List list)
		{

			ListBO listBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Post('{JsonConvert.SerializeObject(list, Formatting.None)}')");

				listBO = new ListBO(_loggerFactory, _config);

				list = listBO.Insert(list);

				response = Ok(list);

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
		/// Editar dados de uma Lista.
		/// </summary>
		/// <returns>retorna dados da Lista atualualizados</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(List), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Put(int id, List list)
		{
			ListBO listBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Put( {id}, '{JsonConvert.SerializeObject(list, Formatting.None)}')");

				listBO = new ListBO(_loggerFactory, _config);

				list.ID = id;
				list = listBO.Update(list);

				response = Ok(list);

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
		/// Excluir uma Lista.
		/// </summary>
		/// <returns></returns>
		/// 
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			ListBO listBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Delete( {id} )");

				listBO = new ListBO(_loggerFactory, _config);
				listBO.Delete(id);

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
