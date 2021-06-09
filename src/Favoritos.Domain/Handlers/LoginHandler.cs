using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Login;
using Favoritos.Domain.Handlers.Contracts;
using Favoritos.Domain.Repositories;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Favoritos.Domain.Handlers
{

    public class LoginHandler :
        Notifiable,
        IHandler<AuthenticateLoginCommand>,
        IHandler<RegisterLoginCommand>,
        IHandler<GetByIdGenericCommand>

    {
        private readonly ILoginRepository _repository;

        public LoginHandler(ILoginRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Handle(GetByIdGenericCommand command)
        {
            throw new NotImplementedException();
        }

        public IActionResult Handle(RegisterLoginCommand command)
        {

            // Fail Fast Validation
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemGetByUserName = _repository.GetByUserName(command.Username);
            if (!(itemGetByUserName is null))
                return GenericCommandResult.BadRequestResult("UserName já está em uso!", command);

            var item = _repository.CreateWithPassword(command.Username, command.Password);

            // Retorna o resultado
            return GenericCommandResult.CreateResult("Login criado!", item);
        }

        public IActionResult Handle(AuthenticateLoginCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemByUserName = _repository.GetByUserName(command.Username);
            if (itemByUserName is null)
                return GenericCommandResult.BadRequestResult("UserName ou Password invalido!", command);

            if (!(_repository.CheckPassword(itemByUserName, command.Password)))
                return GenericCommandResult.BadRequestResult("UserName ou Password invalido!", command);

            // Salva no banco
            var item = _repository.Authenticate(itemByUserName);

            // Retorna o resultado
            return GenericCommandResult.OkResult("Login autenticado!", item);
        }

    }
}
