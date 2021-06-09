
using Favoritos.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using System;


namespace Favoritos.Domain.Commands.Favorito
{
    public class CreateFavoritoCommand : Notifiable, ICommand
    {
        public CreateFavoritoCommand()
        {
        }

        public CreateFavoritoCommand(Guid idCliente, Guid idProduto) : this()
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