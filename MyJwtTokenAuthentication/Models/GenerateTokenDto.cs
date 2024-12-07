using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyJwtTokenAuthentication.Models
{
    public class GenerateTokenDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int ExpiryTime { get; set; }
    }
}
