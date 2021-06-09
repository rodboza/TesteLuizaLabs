
using Favoritos.Domain.Entities;
using System;

namespace Favoritos.Domain.Repositories
{
    public interface IClienteRepository : IBaseRepository<ClienteItem>
    {
        ClienteItem GetByEmail(String email);
    }
}


