using PasswordManager.Services.Abstract;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class HttpManager : IHttpService
    {
        private HttpClient _httpClient;

        public async Task<int> SendHttpPostRequestAsync(string apiUrl, string data)
        {
            //*********  Bu sadece SSL'in gerekli olmadığı durumlarda test aşamasında kullanılabilir. ***********
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            //***************************************************************************************************

            _httpClient = new HttpClient(handler);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }
}
