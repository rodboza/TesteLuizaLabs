using Favoritos.Domain.Commands.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Favoritos.Domain.Handlers.Contracts
{
    public interface IHandler<T> where T : ICommand
    {
        IActionResult Handle(T command);
    }




}