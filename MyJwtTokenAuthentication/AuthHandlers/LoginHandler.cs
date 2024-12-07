using MyJwtTokenAuthentication.Models;
using System.Security.Cryptography;

namespace MyJwtTokenAuthentication.AuthHandlers
{
    public class LoginHandler
    {
        public LoginHandler() { }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public User Register(UserRegisterDto userRegisterDto)
        {
              
            CreatePasswordHash(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new()
            {
                Username = userRegisterDto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = userRegisterDto.Email
            };

            return user;
        }

        public bool Login(UserDto userDto)
        {
            return true;
        }
    }
}
