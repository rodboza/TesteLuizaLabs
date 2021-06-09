using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;

namespace Favoritos.Domain.Commands
{
    public class GetByIdGenericCommand : Notifiable, ICommand
    {
        public GetByIdGenericCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public void Validate()
        {
            AddNotifications(
                    new Contract()
                    .Requires()
                    .AreNotEquals(Id, Guid.Empty,"Id","Não pode ser null")
                );
        }
    }
}
