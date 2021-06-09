using Favoritos.Domain.Entities;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using System;
using System.Linq;

namespace Favoritos.Infra.Repositories
{
    public class ClienteRepository : BaseRepository<ClienteItem>, IClienteRepository
    {
        public ClienteRepository(DataContext context) : base(context)
        {

        }

        public ClienteItem GetByEmail(String email)
        {
            return DbSet
                .FirstOrDefault(x => x.Email == email);
        }
    }
}
