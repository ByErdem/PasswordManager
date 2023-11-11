using Newtonsoft.Json;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class RabbitMQManager : IRabbitMQService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHttpService _httpService;
        private readonly string _rabbitMQApiUrl;


        public RabbitMQManager(IConfigurationService configurationService, IHttpService httpService)
        {

            _configurationService = configurationService;
            _rabbitMQApiUrl = _configurationService.GetSetting("rabbitMQApiUrl");
            _httpService = httpService;
        }


        public async Task<string> ReceiveMessage()
        {
            await Task.Run(() => { });
            return null;
            //throw new NotImplementedException();
        }

        public async Task<int> SendMessage(string header, string message)
        {
            var dto = new RabbitDto
            {
                Header = header,
                Message = message
            };
            var result = await _httpService.SendHttpPostRequestAsync(_rabbitMQApiUrl + "Rabbit/SendMessage", JsonConvert.SerializeObject(dto));
            return result;
        }
    }
}
