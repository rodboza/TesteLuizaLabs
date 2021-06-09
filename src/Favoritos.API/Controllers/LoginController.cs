using Favoritos.Domain.Commands.Login;
using Favoritos.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Favoritos.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        readonly LoginHandler _handler;
        public LoginController(LoginHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("teste")]
        public IActionResult GetById()
        {
            return Ok("Teste OK!!!");
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public IActionResult Register([FromBody]RegisterLoginCommand command)
        {
            return _handler.Handle(command);

        }

        [AllowAnonymous]
        [HttpPost("")]
        public IActionResult Authenticate([FromBody]AuthenticateLoginCommand command)
        {
            return _handler.Handle(command);

        }


    }
}
