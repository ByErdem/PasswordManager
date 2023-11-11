
namespace PasswordManager.Entity.Concrete
{
    public class MUser
    {
        public int USERID { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string USERNAME { get; set; }
        public string HASHPASSWORD { get; set; }
        public string SALTPASSWORD { get; set; }
    }
}