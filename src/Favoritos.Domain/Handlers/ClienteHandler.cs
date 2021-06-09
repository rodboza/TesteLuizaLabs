
using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Cliente;
using Favoritos.Domain.Entities;
using Favoritos.Domain.Handlers.Contracts;
using Favoritos.Domain.Repositories;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Favoritos.Domain.Handlers
{
    public class ClienteHandler :
        Notifiable,
        IHandler<GetByEmailClienteCommnad>,
        IHandler<GetByIdGenericCommand>,
        IHandler<GetAllGenericCommand>,
        IHandler<UpdateClienteCommand>,
        IHandler<DeleteClienteCommand>,
        IHandler<CreateClienteCommand>
    //IHandler<MarkTodoAsUndoneCommand>
    {
        private readonly IClienteRepository _repository;

        public ClienteHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Handle(CreateClienteCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemGetByEmail = _repository.GetByEmail(command.Email);
            if (!(itemGetByEmail is null))
                return GenericCommandResult.BadRequestResult("Email já está em uso!", command);

            var item = new ClienteItem(command.Nome, command.Email);

            // Salva no banco
            _repository.Create(item);

            // Retorna o resultado
            return GenericCommandResult.CreateResult("Cliente criado!", item);
        }

        public IActionResult Handle(UpdateClienteCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemGetByEmail = _repository.GetByEmail(command.Email);

            if (!(itemGetByEmail is null))
            {
                if (command.Id != itemGetByEmail.Id)
                    return GenericCommandResult.BadRequestResult("Email já está em uso!", command);
            }

            var item = _repository.GetById(command.Id);

            if (item is null)
                return GenericCommandResult.NotFoundResult("Id não encontrado!", command);

            item.Nome = command.Nome;
            item.Email = command.Email;

            // Salva no banco
            _repository.Update(item);

            // Retorna o resultado
            return GenericCommandResult.OkResult("Cliente atualizado", item);
        }

        public IActionResult Handle(DeleteClienteCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var item = _repository.GetById(command.Id);

            if (item is null)
                return GenericCommandResult.NotFoundResult("Id não encontrado!", command);

            // Salva no banco
            _repository.Remove(item);

            // Retorna o resultado
            return GenericCommandResult.OkResult("Cliente removido", item);

        }

        public IActionResult Handle(GetAllGenericCommand command)
        {
            var items = _repository.GetAll();

            if (items.Count() == 0)
                return GenericCommandResult.NotFoundResult("Não há itens para retrono!", command);


            return GenericCommandResult.OkResult("Lista de Clientes Recuperado!", items);
        }

        public IActionResult Handle(GetByIdGenericCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var item = _repository.GetById(command.Id);

            if (item is null)
                return GenericCommandResult.NotFoundResult("Id não encontrado!", command);

            return GenericCommandResult.OkResult("Cliente recuperado pelo Id", item);
        }

        public IActionResult Handle(GetByEmailClienteCommnad command)
        {
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var item = _repository.GetByEmail(command.Email);

            if (item is null)
                return GenericCommandResult.NotFoundResult("Id não encontrado!", command);


            return GenericCommandResult.OkResult("Cliente recuperado pelo e-mail", item);
        }
    }
}