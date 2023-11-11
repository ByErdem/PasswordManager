using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.RabbitMQAPI.Model;
using RabbitMQ.Client;
using System.Text;

namespace PasswordManager.RabbitMQAPI.Controllers
{
    [ApiController]
    [Route("api/controller/Rabbit")]
    public class RabbitController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RabbitController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("SendMessage")]
        public int SendMessage(RabbitDto dto)
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.Uri = new Uri(_configuration.GetValue<string>("rabbitMQConnectionString"));
                using (var conn = factory.CreateConnection())
                {
                    var channel = conn.CreateModel();
                    channel.QueueDeclare(dto.Header, true, false, false);
                    var body = Encoding.UTF8.GetBytes(dto.Message);
                    channel.BasicPublish(String.Empty, dto.Header, null, body);
                }

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

    }
}
