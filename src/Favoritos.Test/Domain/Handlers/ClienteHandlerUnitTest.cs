using Favoritos.Domain.Commands;
using Favoritos.Domain.Commands.Cliente;
using Favoritos.Domain.Entities;
using Favoritos.Domain.Handlers;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Favoritos.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Favoritos.Test.Domain.Handlers
{
    public class ClienteHandlerUnitTest
    {
        readonly ClienteHandler clienteHandler;
        readonly IClienteRepository clienteRepository;

        public ClienteHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("ClienteHandlerUnitTest").Options;
            DataContext dataContext = (DataContext)new DataContext(options);

            clienteRepository = new ClienteRepository(dataContext);
            clienteHandler = new ClienteHandler(clienteRepository);

            IncluiCliente("212d0f07-8f56-0708-971c-41ee78aadf2b", "teste", "teste@teste.com");
            IncluiCliente("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f", "teste2", "teste2@teste.com");
        }

        private void IncluiCliente(string id, string nome, string email)
        {
            var guid = Guid.Parse(id);
            var item = clienteRepository.GetById(guid);
            if (item is null)
                clienteRepository.Create(new ClienteItem(nome, email) { Id = guid });
        }

        [Theory]
        [InlineData("","", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("teste", "teste@teste.com", "Email já está em uso!", StatusCodes.Status400BadRequest, false)]
        [InlineData("rodrigo","rodrigo@teste.com", "Cliente criado!", StatusCodes.Status201Created, true)]
        public void CreateClienteCommandTest(string nome, string email, string msgResult, int statusCodeResult, bool statusResult)
        {
            var command = new CreateClienteCommand(nome, email);
            var objectResult = (ObjectResult) clienteHandler.Handle(command);
            var result = (GenericCommandResult) objectResult.Value;
            
            Assert.Equal(statusCodeResult, objectResult.StatusCode );
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);


        }

        [Theory]
        [InlineData("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f", "","", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("571fa8cc-2ee7-5ab4-b388-06d55fd8ab2f","teste","teste@teste.com", "Email já está em uso!", StatusCodes.Status400BadRequest, false)]
        [InlineData("a0e4aa47-b17c-f266-f4d6-aba26ec085aa", "teste4","teste4@teste.com", "Id não encontrado!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "teste","teste@teste.com", "Cliente atualizado", StatusCodes.Status200OK, true)]
        public void UpdateClienteCommandTest(string id, string nome, string email, string msgResult, int statusCodeResult, bool statusResult)
        {
            var tryParseGuid = Guid.TryParse(id, out Guid guid);
            Assert.True(tryParseGuid,"Erro ao converter o GUID");
            var command = new UpdateClienteCommand(guid, nome, email);
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }
        [Theory]
        [InlineData("", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("a0e4aa47-b17c-f266-f4d6-aba26ec085aa", "Id não encontrado!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "Cliente removido", StatusCodes.Status200OK, true)]
        public void DeleteClienteCommandTest(string id, string msgResult, int statusCodeResult, bool statusResult)
        {
            Guid.TryParse(id, out Guid guid);
            var command = new DeleteClienteCommand(guid);
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }
        [Fact]
        public void GetAllGenericCommandComDadosTest( )
        {
            string msgResult = "Lista de Clientes Recuperado!";
            int statusCodeResult = StatusCodes.Status200OK;
            bool statusResult = true;

            var command = new GetAllGenericCommand();
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }

        [Fact]
        public void GetAllGenericCommandSemDadosTest()
        {
            string msgResult = "Não há itens para retrono!";
            int statusCodeResult = StatusCodes.Status404NotFound;
            bool statusResult = false;

            foreach (var item in clienteRepository.GetAll())
            {
                clienteRepository.Remove(item);
            }

            var command = new GetAllGenericCommand();
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }


        [Theory]
        [InlineData("", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("a0e4aa47-b17c-f266-f4d6-aba26ec085aa", "Id não encontrado!", StatusCodes.Status404NotFound, false)]
        [InlineData("212d0f07-8f56-0708-971c-41ee78aadf2b", "Cliente recuperado pelo Id", StatusCodes.Status200OK, true)]
        public void GetByIdGenericCommandTest(string id, string msgResult, int statusCodeResult, bool statusResult)
        {
            Guid.TryParse(id, out Guid guid);
            var command = new GetByIdGenericCommand(guid);
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }

        [Theory]
        [InlineData("", "Campos inválidos!", StatusCodes.Status400BadRequest, false)]
        [InlineData("a0e4aa47-b17c-f266-f4d6-aba26ec085aa", "Id não encontrado!", StatusCodes.Status404NotFound, false)]
        [InlineData("teste@teste.com", "Cliente recuperado pelo e-mail", StatusCodes.Status200OK, true)]
        public void GetByEmailClienteCommnadTest(string email, string msgResult, int statusCodeResult, bool statusResult)
        {
            var command = new GetByEmailClienteCommnad(email);
            var objectResult = (ObjectResult)clienteHandler.Handle(command);
            var result = (GenericCommandResult)objectResult.Value;

            Assert.Equal(statusCodeResult, objectResult.StatusCode);
            Assert.Contains(msgResult, result.Message);
            Assert.Equal(statusResult, result.Success);

        }       

    }
}
