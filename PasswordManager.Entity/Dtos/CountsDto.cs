using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class CountsDto
    {
        public int UserCount { get; set; }
        public int CategoryCount { get; set; }
        public int PasswordsCount { get; set; }
    }
}
