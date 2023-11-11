using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class ProductPropertyDto
    {
        public string PROPERTYID { get; set; }
        public int PRODUCTID { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
    }
}
