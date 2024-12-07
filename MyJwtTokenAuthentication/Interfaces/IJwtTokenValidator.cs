using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyJwtTokenAuthentication.Interfaces
{
    public interface IJwtTokenValidator
    {
        public ClaimsPrincipal? ValidateToken(string token);
    }
}
