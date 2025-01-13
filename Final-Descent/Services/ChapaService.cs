using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Final_Descent.Services
{
    public class ChapaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _publicKey;

        public ChapaService(string publicKey)
        {
            _publicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        }

        public ChapaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> InitializeTransactionAsync(string amount, string phoneNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.chapa.co");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _publicKey);

                var requestBody = new
                {
                    amount = amount,
                    currency = "ETB",
                    email = "testuser3@gmail.com",
                    first_name = "Test",
                    last_name = "User",
                    phone_number = phoneNumber,
                    tx_ref = Guid.NewGuid().ToString(),
                    callback_url = "https://localhost:7287/Payment/Callback",
                    return_url = (string?)null
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"[DEBUG] Sending Request: {json}");
                Console.WriteLine($"[DEBUG] Headers: {client.DefaultRequestHeaders}");

                var response = await client.PostAsync("/v1/transaction/initialize", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] Response: {responseContent}");

                response.EnsureSuccessStatusCode();
                return responseContent;
            }
        }
    }
}
