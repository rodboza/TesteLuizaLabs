using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Favorito;
using Favoritos.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Favoritos.API.Controllers
{
    [Authorize]
    [Route("api/v1/Cliente/{idCliente}/[controller]")]
    [ApiController]
    public class FavoritoController : ControllerBase
    {

        readonly FavoritoHandler _handler;
        public FavoritoController(FavoritoHandler handler)
        {
            _handler = handler;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetAll([FromRoute] Guid idCliente)
        {
            var command = new GetByIdGenericCommand(idCliente);
            return _handler.Handle(command);
        }

        [Route("")]
        [HttpPost]
        public IActionResult Create([FromRoute]Guid idCliente, [FromBody]CreateFavoritoCommand command)
        {
            command.IdCliente = idCliente;
            return _handler.Handle(command);
        }

        [Route("{idProduto}")]
        [HttpDelete]
        public IActionResult Delete([FromRoute]DeleteFavoritoCommand command)
        {
            return _handler.Handle(command);
        }

    }
}
