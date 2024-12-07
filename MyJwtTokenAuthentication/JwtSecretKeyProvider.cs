using MyJwtTokenAuthentication.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJwtTokenAuthentication
{
    public interface IJwtSecretKeyProvider
    {
        public string GetSecretKey();
        void SetSecretKey(string secretKey);
    }
    public class JwtSecretKeyProvider : IJwtSecretKeyProvider
    {
        private string _secretKey;

        public JwtSecretKeyProvider(IOptions<JwtSettings> options)
        {
            _secretKey = options.Value.SecretKey;
        }

        public string GetSecretKey() => _secretKey;

        public void SetSecretKey(string secretKey) => _secretKey = secretKey;
    }
}
