using PasswordManager.Services.Abstract;
using System.Configuration;

namespace PasswordManager.Services.Concrete
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
