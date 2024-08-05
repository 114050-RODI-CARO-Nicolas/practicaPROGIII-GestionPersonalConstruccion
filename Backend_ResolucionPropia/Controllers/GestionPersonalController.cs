using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parcial.Dtos;
using Parcial.Servicios.Interfaces;

namespace Parcial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionPersonalController : ControllerBase
    {
        private readonly IGestionPersonalService _gestionPersonalService;

        public GestionPersonalController(IGestionPersonalService gestionPersonalService)
        {
            _gestionPersonalService = gestionPersonalService;
        }

        [HttpGet("obras")]
        public async Task<IActionResult> GetObras()
        {
           var response = await _gestionPersonalService.GetObrasAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);    

        }


        [HttpGet("albaniles")]
        public async Task<IActionResult> GetAlbaniles([FromQuery] Guid obraId)
        {
            var response = await _gestionPersonalService.GetAlbanilesNotInObraAsync(obraId);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);    
        }


        [HttpPost("albanil")]
        public async Task<IActionResult> PostAlbanil([FromBody] AlbanilDto albanilDto)
        {
            var response = await _gestionPersonalService.PostAlbanilAsync(albanilDto);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("albanilxobra")]
        public async Task<IActionResult> PostAlbanilXObra([FromBody] AlbanilXObraDto albanilXObraDto)
        {
            var response = await _gestionPersonalService.PostAlbanilXObraAsync(albanilXObraDto);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
