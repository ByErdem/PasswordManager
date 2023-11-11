using PasswordManager.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class CategoryDto : EntityBase
    {
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public int PARENTCATEGORYID { get; set; }
        public string PARENTCATEGORYNAME { get; set; }
    }
}
