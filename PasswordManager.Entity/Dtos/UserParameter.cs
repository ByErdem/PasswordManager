using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class UserParameter
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string SecretKey { get; set; }
        public string GuidKey { get; set; }
    }
}
