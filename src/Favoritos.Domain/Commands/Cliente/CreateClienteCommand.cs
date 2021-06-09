using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;


namespace Favoritos.Domain.Commands.Cliente
{
    public class CreateClienteCommand : Notifiable, ICommand
    {
        public CreateClienteCommand()
        {
        }

        public CreateClienteCommand(string nome, string email) : this()
        {
            Nome = nome;
            Email = email;

        }

        public string Nome { get; set; }
        public string Email { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                        .Requires()
                        .HasMinLen(Nome, 1, "Nome", "Nome inválido!")
                        .HasMinLen(Email, 1, "Email", "E-mail inválido!"));
        }
    }
}