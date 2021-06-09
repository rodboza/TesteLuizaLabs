using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Favorito;
using Favoritos.Domain.Entities;
using Favoritos.Domain.Handlers;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Favoritos.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Favoritos.Test.Domain.Handlers
{
    public class FavoritoHandlerUnitTest
    {
        readonly FavoritoHandler handler;
        readonly IClienteRepository clienteRepository;
        readonly IFavoritoRepository favoritoRepository;


        public FavoritoHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("FavoritoHandlerUnitTest").Options;
            DataContext dataContext = (DataContext)new DataContext(options);

            clienteRepository = new ClienteRepository(dataContext);
            favoritoRepository = new FavoritoRepository(dataContext);
            handler = new FavoritoHandler(favoritoRepository, clienteRepository);

            IncluiCliente("212d0f07-8f56-0708-971c-41ee78aadf2b", "teste", "teste@teste.com", new string[] { "e9a72482-7e95-44ff-ea5a-75147aef2184", "958ec015-cfcf-258d-c6df-1721de0ab6ea" });
            IncluiCliente("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f", "teste2", "teste2@teste.com", new string[] { });
        }

        private void IncluiCliente(string id, string nome, string email, string[] produtos)
        {
            var guid = Guid.Parse(id);
            var item = clienteRepository.GetById(guid);
            if (item is null)
            {
                var cliente = new ClienteItem(nome, email) { Id = guid };
                foreach (var p in produtos)
                {
                    var guidP = Guid.Parse(p);
                    var produto = new FavoritoItem(guidP, p, "brand", null, 0, 0) { Id = guidP };
                    favoritoRepository.Create(produto);
                    cliente.ListaProdutosFavoritos.Add(produto);
                }
                clienteRepository.Create(cliente);
            }
        }

        [Theory]
        [InlineData("", "", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", "adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", "IdCliente não existe!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "16bf275d-2fd6-43ce-8393-be60018b2821", "IdProduto não existe!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "e9a72482-7e95-44ff-ea5a-75147aef2184", " já existe na lista do Cliente", StatusCodes.Status400BadRequest, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", " adicionado ao cliente!", StatusCodes.Status201Created, true)]
        public void CreateFavoritoCommandTest(string idCliente, string idProduto, string msgResult, int statusCodeResult, bool statusResult)
        {
            Guid.TryParse(idProduto, out Guid guidP);
            Guid.TryParse(idCliente, out Guid guidC);
            var command = new CreateFavoritoCommand(guidC, guidP);
            var objectResult = (ObjectResult)handler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }

        [Theory]
        [InlineData("", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", "IdCliente não existe!", StatusCodes.Status404NotFound, false)]
        [InlineData("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f", "Não há itens para retrono!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "Lista de Favoritos do cliente", StatusCodes.Status200OK, true)]
        public void GetByIdGenericCommandTest(string id, string msgResult, int statusCodeResult, bool statusResult)
        {
            Guid.TryParse(id, out Guid guid);
            var command = new GetByIdGenericCommand(guid);
            var objectResult = (ObjectResult)handler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }



        [Theory]
        [InlineData("", "", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", "adc14fd0-3dc0-ab07-325b-8cf144ca9b0f", "IdCliente não existe!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "5ab740b8-4046-3f19-ae21-3302dd7abef8", " não existe na lista do Cliente", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "e9a72482-7e95-44ff-ea5a-75147aef2184", " removido do cliente!", StatusCodes.Status200OK, true)]
        public void DeleteFavoritoCommandTest(string idCliente, string idProduto, string msgResult, int statusCodeResult, bool statusResult)
        {
            Guid.TryParse(idProduto, out Guid guidP);
            Guid.TryParse(idCliente, out Guid guidC);
            var command = new DeleteFavoritoCommand(guidC, guidP);
            var objectResult = (ObjectResult)handler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }


        [Theory]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b")]
        [InlineData("adc14fd0-3dc0-ab07-325b-8cf144ca9b0f")]
        public void BuscaItemFavoritoTest(string idProduto) {

            Guid.TryParse(idProduto, out Guid guidP);
            var produto = FavoritoHandler.BuscaItemFavorito(guidP);
            Assert.NotNull(produto);
        }


    }
}
