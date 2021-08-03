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

	public class CardController : ControllerBase
	{
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILogger<CardController> _log;
		private readonly IConfiguration _config;

		public CardController(ILoggerFactory loggerFactory, IConfiguration config)
		{
			_loggerFactory = loggerFactory;
			_log = loggerFactory.CreateLogger<CardController>();
			_config = config;
		}

		/// <summary>
		/// Retornar todos os cards.
		/// </summary>
		/// <returns>retorna dados dos usários</returns>

		[HttpGet]
		[ProducesResponseType(typeof(Card), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get()
		{
			CardBO cardBO;
			List<Card> cards;
			ObjectResult response;

			try
			{
				_log.LogInformation("Starting Get()");
				cards = new List<Card>();

				cardBO = new CardBO(_loggerFactory, _config);
				cards = cardBO.Get();

				response = Ok(cards);

				_log.LogInformation($"Finishing Get() with {cards.Count} results");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}


		/// <summary>
		/// Retornar todos os cards a partir de um List ID.
		/// </summary>
		/// <returns>retorna dados dos usários</returns>
		[HttpGet ("{id}/list")]
		[ProducesResponseType(typeof(List<Card>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Get(int id)
		{
			CardBO cardBO;
			List<Card> cards;
			ObjectResult response;

			try
			{
				_log.LogInformation("Starting Get()");
				cards = new List<Card>();

				cardBO = new CardBO(_loggerFactory, _config);
				cards = cardBO.Get(id);

				response = Ok(cards);

				_log.LogInformation($"Finishing Get() with {cards.Count} results");
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message);
				response = StatusCode(500, ex.Message);
			}

			return response;
		}



		/// <summary>
		/// Criar um card.
		/// </summary>
		/// <returns>dados da lista criada</returns>

		[HttpPost]
		[ProducesResponseType(typeof(Card),StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Post([FromBody] Card card)
		{

			CardBO cardBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Post('{JsonConvert.SerializeObject(card, Formatting.None)}')");

				cardBO = new CardBO(_loggerFactory, _config);

				card = cardBO.Insert(card);

				response = Ok(card);

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
		/// Editar dados de Card.
		/// </summary>
		/// <returns>retorna dados do usário atualualizados</returns>
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Put(int id, Card card)
		{
			CardBO cardBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Put( {id}, '{JsonConvert.SerializeObject(card, Formatting.None)}')");

				cardBO = new CardBO(_loggerFactory, _config);

				card.ID = id;
				card = cardBO.Update(card);

				response = Ok(card);

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
		/// Excluir Card.
		/// </summary>
		/// <returns></returns>
		/// 
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult Delete(int id)
		{
			CardBO cardBO;
			ObjectResult response;

			try
			{
				_log.LogInformation($"Starting Delete( {id} )");

				cardBO = new CardBO(_loggerFactory, _config);
				cardBO.Delete(id);

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
