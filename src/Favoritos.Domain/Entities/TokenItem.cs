using System;

namespace Favoritos.Domain.Entities
{
    public class TokenItem
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public Guid IdUser { get; set; }

        public TokenItem(string username, string token, Guid idUser)
        {
            Username = username;
            Token = token;
            IdUser = idUser;
        }
    }
}
