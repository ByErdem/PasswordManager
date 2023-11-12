using PasswordManager.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManager.Services.Abstract;

namespace PasswordManager.Services.Concrete
{
    public class DbContextEntity : DbContext, IDbContextEntity
    {
        public DbContextEntity()
            : base(ConnectionString())
        {
        }

        public static string ConnectionString()
        {
            string server = "DESKTOP-JCSFFJ8";
            string databaseName = "PasswordManager";
            string userName = "sa";
            string password = "123";
            string model = databaseName + "Model";
            string metaData = "metadata=res://*/" + model + ".csdl|res://*/" + model + ".ssdl|res://*/" + model + ".msl";
            string provider = "provider=System.Data.SqlClient";
            return $"{metaData};{provider};provider connection string=\"data source={server};database={databaseName};user id={userName};password={password};MultipleActiveResultSets=True;App=EntityFramework\"";
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<CATEGORY> CATEGORY { get; set; }
        public virtual DbSet<USER> USER { get; set; }
        public virtual DbSet<MYPASSWORDS> MYPASSWORDS { get; set; }
    }
}
