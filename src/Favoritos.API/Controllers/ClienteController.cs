using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Cliente;
using Favoritos.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Favoritos.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        readonly ClienteHandler _handler;
        public ClienteController(ClienteHandler handler)
        {
            _handler = handler;
        }


        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return _handler.Handle(new GetAllGenericCommand());
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute]Guid id)
        {
            return _handler.Handle(new GetByIdGenericCommand(id));
        }


        [Route("")]
        [HttpPost]
        public IActionResult Create([FromBody]CreateClienteCommand command)
        {
            return _handler.Handle(command);
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult Update([FromRoute]Guid id, [FromBody]UpdateClienteCommand command)
        {
            command.Id = id;
            return _handler.Handle(command);
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete([FromRoute]DeleteClienteCommand command)
        {
            return _handler.Handle(command);
        }





    }
}