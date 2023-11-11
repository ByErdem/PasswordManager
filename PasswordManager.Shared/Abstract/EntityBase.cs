using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Shared.Abstract
{
    public abstract class EntityBase
    {
        public virtual bool ISDELETED { get; set; }
        public virtual DateTime CREATEDDATE { get; set; }
        public virtual int CREATORUSERID { get; set; }
    }
}
