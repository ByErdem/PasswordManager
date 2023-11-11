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
            _httpClient = new HttpClient();
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
