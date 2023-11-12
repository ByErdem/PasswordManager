using PasswordManager.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IDbContextEntity
    {
        DbSet<CATEGORY> CATEGORY { get; set; }
        DbSet<USER> USER { get; set; }
        DbSet<MYPASSWORDS> MYPASSWORDS { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
    }
}
