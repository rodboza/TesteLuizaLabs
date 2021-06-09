using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;


namespace Favoritos.Domain.Commands.Favorito
{
    public class DeleteFavoritoCommand : Notifiable, ICommand
    {
        public DeleteFavoritoCommand()
        {
        }

        public DeleteFavoritoCommand(Guid idCliente, Guid idProduto) : this()
        {
            IdCliente = idCliente;
            IdProduto = idProduto;

        }

        public Guid IdCliente { get; set; }
        public Guid IdProduto { get; set; }

        public void Validate()
        {
            AddNotifications(
               new Contract()
                   .Requires()
                   .AreNotEquals(IdCliente, Guid.Empty, "IdCliente", "Não pode ser null")
                   .AreNotEquals(IdProduto, Guid.Empty, "IdProduto", "Não pode ser null")
                   );
        }
    }
}