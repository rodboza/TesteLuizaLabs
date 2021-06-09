using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;

namespace Favoritos.Domain.Commands.Cliente
{
    public class UpdateClienteCommand : Notifiable, ICommand
    {
        public UpdateClienteCommand() { }

        public UpdateClienteCommand(Guid id, string nome, string email)
        {
            Nome = nome;
            Email = email;
            Id = id;
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                    .Requires()
                    .HasMinLen(Nome, 1, "Nome", "Nome inválido!")
                    .HasMinLen(Email, 1, "Email", "E-mail inválido!"));
        }
    }
}




