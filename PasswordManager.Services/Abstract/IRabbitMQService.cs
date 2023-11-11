using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IRabbitMQService
    {
        Task<int> SendMessage(string header, string message);
        Task<string> ReceiveMessage();
    }
}
