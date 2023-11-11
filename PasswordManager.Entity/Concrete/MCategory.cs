using PasswordManager.Shared.Abstract;

namespace PasswordManager.Entity.Concrete
{
    public class MCategory:EntityBase
    {
        public int CATEGORYID { get; set; }
        public string CATEGORYNAME { get; set; }
        public int PARENTCATEGORYID { get; set; }
    }
}