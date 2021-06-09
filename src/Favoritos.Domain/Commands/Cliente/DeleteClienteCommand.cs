using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Favoritos.Domain.Commands.Cliente
{
    public class DeleteClienteCommand : Notifiable, ICommand
    {
        public DeleteClienteCommand() { }

        public DeleteClienteCommand(Guid id)
        {
            Id = id;
        }

        [Required]
        public Guid Id { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract().Requires().AreNotEquals(Id, Guid.Empty,"Id","Não pode ser null"));
        }
    }
}
