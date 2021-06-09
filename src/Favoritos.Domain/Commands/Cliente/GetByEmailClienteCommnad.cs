using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;



namespace Favoritos.Domain.Commands.Cliente
{
    public class GetByEmailClienteCommnad : Notifiable, ICommand
    {
        public GetByEmailClienteCommnad(string email)
        {
            Email = email;

        }
        public String Email { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                    .Requires()
                    .HasMinLen(Email, 1, "Email", "E-mail inválido!"));
        }
    }
}


