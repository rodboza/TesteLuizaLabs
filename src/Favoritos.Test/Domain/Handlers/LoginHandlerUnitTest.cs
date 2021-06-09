using Favoritos.Domain.Handlers;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Favoritos.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Favoritos.Test.Domain.Handlers
{
    public class LoginHandlerUnitTest
    {
        readonly LoginHandler handler;
        readonly ILoginRepository repository;


        public LoginHandlerUnitTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("LoginHandlerUnitTest").Options;
            DataContext dataContext = (DataContext)new DataContext(options);

            repository = new LoginRepository(dataContext);
            handler = new LoginHandler(repository);

        }



    }
}
