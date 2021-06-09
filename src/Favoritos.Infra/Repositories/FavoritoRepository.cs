
using Favoritos.Domain.Entities;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using System;
using System.Linq;

namespace Favoritos.Infra.Repositories
{
    public class FavoritoRepository : BaseRepository<FavoritoItem>, IFavoritoRepository
    {
        public FavoritoRepository(DataContext context) : base(context)
        {

        }

        public FavoritoItem GetByIdProdutoOrigem(Guid idOrigem)
        {
            return DbSet
                .FirstOrDefault(x => x.IdOrigem == idOrigem);
        }
    }
}
