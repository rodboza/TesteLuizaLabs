using Favoritos.Domain.Commands.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Favoritos.Domain.Commands
{
    public class GenericCommandResult : ICommandResult
    {
        public GenericCommandResult() { }

        public GenericCommandResult(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }


        public static IActionResult CreateResult(string msg, Object item)
        {
            return new CreatedResult("", new GenericCommandResult(true, msg, item));
        }

        public static IActionResult BadRequestResult(string msg, Object item)
        {
            return new BadRequestObjectResult(new GenericCommandResult(false, "Houston, we have a problem! " + msg, item));
        }

        public static IActionResult NotFoundResult(string msg, Object item)
        {
            return new NotFoundObjectResult(new GenericCommandResult(false, "Houston, we have a problem! " + msg, item));
        }

        public static IActionResult OkResult(string msg, Object item)
        {
            return new OkObjectResult(new GenericCommandResult(true, msg, item));
        }

        public static IActionResult InternalServerErrorResult(string msg, Object item)
        {
            var saida = new ObjectResult(new GenericCommandResult(false, "Houston, we have a problem! " + msg, item))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return saida;
        }
        public static IActionResult UnauthorizedResult(string msg, Object item)
        {
            return new UnauthorizedObjectResult(new GenericCommandResult(false, msg, item));
        }

    }
}