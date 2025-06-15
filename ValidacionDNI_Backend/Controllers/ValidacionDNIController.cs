using ValidacionDNI_Backend.BusinessLogic;
using ValidacionDNI_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
namespace ValidacionDNI_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidacionDNIController: ControllerBase
    {
        private ValidacionDNIHandler vgDataAccess;
        private beMySettings vgSettings;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ValidacionDNIController> _logger;

        public ValidacionDNIController(IOptions<beMySettings> peSettings, IConfiguration configuration, ILogger<ValidacionDNIController> logger, IWebHostEnvironment env)
        {
            vgSettings = peSettings.Value;
            _logger = logger;
            vgDataAccess = new ValidacionDNIHandler(vgSettings, env);
            _configuration = configuration;
        }


        [HttpPost("registrar")]
        public IActionResult RegistrarDNI([FromBody] PostulanteDTO postulante)
        {
            try
            {
                var resultado = vgDataAccess.RegistrarDNIAsync(postulante).Result;

                if (resultado != null)
                {
                    _logger.LogInformation("DNI Registrado");
                    return Ok(new { resultado.Mensaje, IdTipoMensaje = resultado.IdTipoMensaje });
                }

                return BadRequest("No se pudo realizar el registro");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { IdTipoMensaje = 1, Message = ex.Message });
            }
        }
        [HttpGet("listaTipoDocumento")]
        public async Task<ActionResult<TipoDocumentoLista>> ListarTipoDocumento()
        {
            try
            {
                var resultado = await vgDataAccess.ListaDocumentoAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar tipo de documento");
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }

        [HttpGet("listaModalidad")]
        public async Task<ActionResult<ModalidadTipoLista>> ListarModalidad()
        {
            try
            {
                var resultado = await vgDataAccess.ModalidadListaAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar tipo de documento");
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }

        [HttpGet("listaSede")]
        public async Task<ActionResult<SedesLista>> ListarSede()
        {
            try
            {
                var resultado = await vgDataAccess.SedeListaAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar sedes");
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }

        [HttpGet("listaEscuela")]
        public async Task<ActionResult<EscuelasLista>> ListarEscuela()
        {
            try
            {
                var resultado = await vgDataAccess.EscuelaListaAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar tipo de escuela");
                return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
            }
        }

    }
}
