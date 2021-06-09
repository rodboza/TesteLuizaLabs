using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Favorito;
using Favoritos.Domain.Entities;
using Favoritos.Domain.Handlers.Contracts;
using Favoritos.Domain.Repositories;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;


namespace Favoritos.Domain.Handlers
{
    public class FavoritoHandler :
        Notifiable,
        IHandler<DeleteFavoritoCommand>,
        IHandler<CreateFavoritoCommand>,
        IHandler<GetByIdGenericCommand>

    {
        private readonly IFavoritoRepository _favoritoRepository;
        private readonly IClienteRepository _clienteRepository;

        public FavoritoHandler(IFavoritoRepository favorito, IClienteRepository cliente)
        {
            _favoritoRepository = favorito;
            _clienteRepository = cliente;
        }



        public IActionResult Handle(CreateFavoritoCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemCliente = _clienteRepository.GetById(command.IdCliente);
            if (itemCliente is null)
                return GenericCommandResult.NotFoundResult("IdCliente não existe!", command);

            var itemFavorito = _favoritoRepository.GetByIdProdutoOrigem(command.IdProduto);
            if (itemFavorito is null)
            {
                itemFavorito = BuscaItemFavorito(command.IdProduto);
                if (itemFavorito is null)
                    return GenericCommandResult.NotFoundResult("IdProduto não existe!", command);
                _favoritoRepository.Create(itemFavorito);
            }

            if (itemCliente.ListaProdutosFavoritos.Contains(itemFavorito))
                return GenericCommandResult.BadRequestResult("IdProduto " + itemFavorito.Id + " já existe na lista do Cliente", itemCliente);

            itemCliente.ListaProdutosFavoritos.Add(itemFavorito);
            // Salva no banco
            _clienteRepository.Update(itemCliente);

            return GenericCommandResult.CreateResult("Favorito " + itemFavorito.Id + " adicionado ao cliente!", itemCliente);
        }

        public IActionResult Handle(GetByIdGenericCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemCliente = _clienteRepository.GetById(command.Id);
            if (itemCliente is null)
                return GenericCommandResult.NotFoundResult("IdCliente não existe!", command);

            var items = _clienteRepository.GetById(command.Id).ListaProdutosFavoritos.ToList();

            if (items.Count() == 0)
                return GenericCommandResult.NotFoundResult("Não há itens para retrono!", command);

            return GenericCommandResult.OkResult("Lista de Favoritos do cliente " + command.Id + " Recuperado!", items);
        }


        public IActionResult Handle(DeleteFavoritoCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return GenericCommandResult.BadRequestResult("Campos inválidos!", command.Notifications);

            var itemCliente = _clienteRepository.GetById(command.IdCliente);
            if (itemCliente is null)
                return GenericCommandResult.NotFoundResult("IdCliente não existe!", command);

            var itemFavorito = itemCliente.ListaProdutosFavoritos.Where(i => i.IdOrigem == command.IdProduto).FirstOrDefault();

            if (itemFavorito is null)
                return GenericCommandResult.NotFoundResult("IdProduto " + command.IdProduto + " não existe na lista do Cliente", itemCliente);

            itemCliente.ListaProdutosFavoritos.Remove(itemFavorito);
            _clienteRepository.Update(itemCliente);

            return GenericCommandResult.OkResult("Favorito " + itemFavorito.Id + " removido do cliente!", itemCliente);
        }

        public static FavoritoItem BuscaItemFavorito(Guid idProduto)
        {
            string uri = @"http://challenge-api.luizalabs.com/api/product/" + idProduto + "/";
            HttpClient http = new HttpClient();
            HttpResponseMessage response = http.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                var itemString = response.Content.ReadAsStringAsync().Result;
                var item = JsonDocument.Parse(itemString);
                return new FavoritoItem(
                        idProduto,
                        item.RootElement.GetProperty("title").GetString(),
                        item.RootElement.GetProperty("brand").GetString(),
                        new Uri(item.RootElement.GetProperty("image").GetString()),
                        itemString.Contains("reviewScore") ? item.RootElement.GetProperty("reviewScore").GetDecimal() : 0,
                        item.RootElement.GetProperty("price").GetDecimal()
                );
            }

            return null;
        }

    }
}
