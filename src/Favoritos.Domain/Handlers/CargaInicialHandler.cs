
using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.CargaInicial;
using Favoritos.Domain.Entities;
using Favoritos.Domain.Handlers.Contracts;
using Favoritos.Domain.Repositories;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Favoritos.Domain.Handlers
{
    public class CargaInicialHandler :
        Notifiable,
        IHandler<CargaInicialLoginCommand>
    {
        private readonly IClienteRepository _repoCliente;
        private readonly ILoginRepository _repoLogin;

        public CargaInicialHandler(IClienteRepository repoCliente, ILoginRepository repoLogin)
        {
            _repoCliente = repoCliente;
            _repoLogin = repoLogin;
        }

        public IActionResult Handle(CargaInicialLoginCommand command)
        {
            object item;
            try
            {
                var itemLogin = new LoginItem("teste")
                {
                    Id = Guid.Parse("3c2dbea5-a08b-447d-8e85-7de4cbaa16c0")
                };
                _repoLogin.SetPassword(itemLogin, "123");
                _repoLogin.Create(itemLogin);

                var itemCliente1 = new ClienteItem("teste1", "teste1@teste.com")
                {
                    Id = Guid.Parse("6a153823-37da-49b2-b1f8-22cec0b1d0fd")
                };
                itemCliente1.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f")));
                itemCliente1.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("212d0f07-8f56-0708-971c-41ee78aadf2b")));
                itemCliente1.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("a0e4aa47-b17c-f266-f4d6-aba26ec085aa")));
                itemCliente1.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("2fed4df3-5f11-c6a3-ac69-f9a408f2eff7")));
                _repoCliente.Create(itemCliente1);

                var itemCliente2 = new ClienteItem("teste2", "teste2@teste.com")
                {
                    Id = Guid.Parse("945e07d2-77d3-4b26-b9f5-f4bed95924ee")
                };
                itemCliente2.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("9175d13b-52c6-f14c-05d1-f70f12e908b5")));
                itemCliente2.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("a0e4aa47-b17c-f266-f4d6-aba26ec085aa")));
                itemCliente2.ListaProdutosFavoritos.Add(FavoritoHandler.BuscaItemFavorito(Guid.Parse("9175d13b-52c6-f14c-05d1-f70f12e908b5")));
                _repoCliente.Create(itemCliente2);

                item = new { Login = itemLogin, Cliente1 = itemCliente1, Cliente2 = itemCliente2 };
            }
            catch (System.Exception e)
            {
                return GenericCommandResult.InternalServerErrorResult("Erro na carga inicial!", e);
            }
            return GenericCommandResult.CreateResult("Carga Inicial feita com sucesso!", item);
        }
    }
}
