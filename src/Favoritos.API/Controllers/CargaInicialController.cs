using Favoritos.Domain.Commands.CargaInicial;
using Favoritos.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Favoritos.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CargaInicialController : ControllerBase
    {
        readonly CargaInicialHandler _handler;
        public CargaInicialController(CargaInicialHandler handler)
        {
            _handler = handler;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CargaInicial()
        {
            var command = new CargaInicialLoginCommand();
            return _handler.Handle(command);

        }



    }
}
