using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class MyPasswordDto
    {
        public int ID { get; set; }
        public int USERID { get; set; }
        public string NAME { get; set; }
        public string URL { get; set; }
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; } 
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
