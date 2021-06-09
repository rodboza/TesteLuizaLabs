using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;

namespace Favoritos.Domain.Commands.CargaInicial
{
    public class CargaInicialLoginCommand : Notifiable, ICommand
    {
        public void Validate()
        {
            AddNotifications(
                new Contract()
                .Requires()
            );
        }
    }
}
