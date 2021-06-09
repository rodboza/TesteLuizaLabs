namespace Favoritos.Domain.Entities
{
    public class LoginItem : Entity
    {

        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public LoginItem(string userName, byte[] passwordHash, byte[] passwordSalt) : this(userName)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public LoginItem(string userName)
        {
            UserName = userName;

        }

    }
}
