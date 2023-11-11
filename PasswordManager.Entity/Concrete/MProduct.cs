using PasswordManager.Shared.Abstract;

namespace PasswordManager.Entity.Concrete
{
    public class MProduct : EntityBase
    {
        public int PRODUCTID { get; set; }
        public string PRODUCTNAME { get; set; }
        public int CATEGORYID { get; set; }
        public decimal PRICE { get; set; }
        public string IMAGEPATH { get; set; }
    }
}