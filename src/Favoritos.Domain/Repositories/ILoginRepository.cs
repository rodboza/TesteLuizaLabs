using Favoritos.Domain.Entities;

namespace Favoritos.Domain.Repositories
{
    public interface ILoginRepository : IBaseRepository<LoginItem>
    {
        public LoginItem GetByUserName(string userName);
        public LoginItem CreateWithPassword(string userName, string password);
        public LoginItem SetPassword(LoginItem login, string password);
        public TokenItem Authenticate(LoginItem login);
        public bool CheckPassword(LoginItem login, string password);
    }
}


