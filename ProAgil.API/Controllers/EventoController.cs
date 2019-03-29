using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Model;
using ProAgil.Repository.Repositories;

namespace ProAgil.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repositopry;

        public EventoController(IProAgilRepository repository)
        {
            _repositopry = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repositopry.GetAllEventosAsync(true);
                return Ok(results);
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                var results = await _repositopry.GetEventoAsyncById(EventoId, true);
                return Ok(results);
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await _repositopry.GetAllEventosAsyncByTema(tema, true);
                return Ok(results);
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repositopry.Add(model);

                if (await _repositopry.SaveChangesAsync()) {
                    return Created($"/api/evento/{model.EventoId}", model);
                }
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Post(int EventoId, Evento model)
        {
            try
            {
                var evento = await _repositopry.GetEventoAsyncById(EventoId, false);

                if (evento == null) {
                    return NotFound();
                }
                
                _repositopry.Update(model);

                if (await _repositopry.SaveChangesAsync()) {
                    return Created($"/api/evento/{model.EventoId}", model);
                }
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Post(int EventoId)
        {
            try
            {
                var evento = await _repositopry.GetEventoAsyncById(EventoId, false);

                if (evento == null) {
                    return NotFound();
                }
                
                _repositopry.Delete(evento);

                if (await _repositopry.SaveChangesAsync()) {
                    return Ok();
                }
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }


    }
}