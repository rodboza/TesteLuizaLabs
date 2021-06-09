
using Favoritos.Domain.Entities;
using System;

namespace Favoritos.Domain.Repositories
{
    public interface IFavoritoRepository : IBaseRepository<FavoritoItem>
    {
        public FavoritoItem GetByIdProdutoOrigem(Guid idOrigem);
    }
}


