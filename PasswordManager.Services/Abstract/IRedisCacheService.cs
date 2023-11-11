using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IRedisCacheService
    {
        Task<string> GetAsync(string key);
        Task<bool> SetAsync(string key, string value);
        Task DeleteAsync(string key);
        bool Any();
    }
}
