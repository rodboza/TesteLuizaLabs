using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;

namespace Favoritos.Domain.Commands
{
    public class GetAllGenericCommand : Notifiable, ICommand
    {
        public GetAllGenericCommand()
        {

        }

        public void Validate()
        {
        }
    }
}
