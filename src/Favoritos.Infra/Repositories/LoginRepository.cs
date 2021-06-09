using Favoritos.Domain.Entities;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Favoritos.Infra.Repositories
{


    public class LoginRepository : BaseRepository<LoginItem>, ILoginRepository
    {
        public static string Secret = "My Precious, says Gollum through missing rotted teeth";
        public LoginRepository(DataContext context) : base(context)
        {
        }

        public TokenItem Authenticate(LoginItem login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenItem(login.UserName, tokenString, login.Id);
        }

        public LoginItem CreateWithPassword(string userName, string password)
        {

            var item = SetPassword(new LoginItem(userName), password);

            this.Create(item);
            return item;

        }

        public bool CheckPassword(LoginItem login, string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(login.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != login.PasswordHash[i]) return false;
                }
            }

            return true;
        }

        public LoginItem GetByUserName(string userName)
        {
            return DbSet
                .FirstOrDefault(x => x.UserName == userName);
        }

        public LoginItem SetPassword(LoginItem login, string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password não pode ser vazio ou espaço em branco!", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                login.PasswordHash = hmac.Key;
                login.PasswordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return login;
        }
    }
}