using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System.ComponentModel.DataAnnotations;


namespace Favoritos.Domain.Commands.Login
{
    public class AuthenticateLoginCommand : Notifiable, ICommand
    {
        public AuthenticateLoginCommand()
        {
        }

        public AuthenticateLoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }



        public void Validate()
        {
            AddNotifications(
                new Contract()
                .Requires()
                .HasMinLen(Username, 1, "Username", "Username inválido!")
                .HasMinLen(Password, 1, "Password", "Password inválido!"));
        }
    }
}