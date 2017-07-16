namespace Nancy.Demo.Authentication.JwtBearer.Client
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;

    class Program
    {
        private static string Scheme = "Guest";

        static void Main(string[] args)
        {
            HttpClient _client = new HttpClient();

            _client.DefaultRequestHeaders.Clear();

            TestJwtRequest(_client);

            Console.Read();
        }

        private static void TestJwtRequest(HttpClient _client)
        {
            var client_id = "100";
            var client_secret = "888";
            var username = "Member";
            var password = "123";

            var asUrl = $"http://localhost:65134/api/token/auth?grant_type=password&client_id={client_id}&client_secret={client_secret}&username={username}&password={password}";

            Console.WriteLine("begin authorizing:");

            HttpResponseMessage asMsg = _client.GetAsync(asUrl).Result;

            string result = asMsg.Content.ReadAsStringAsync().Result;

            var responseData = JsonConvert.DeserializeObject<ResponseData>(result);

            if (responseData.Code != "999")
            {
                Console.WriteLine("authorizing fail");
                return;
            }

            var token = JsonConvert.DeserializeObject<Token>(responseData.Data);

            Console.WriteLine("authorizing successfully");
            Console.WriteLine($"the response of authorizing {result}");

            Console.WriteLine("begin to request the resouce server");

            var rsUrl = "http://localhost:60774/";
            _client.DefaultRequestHeaders.Add("Authorization", $"{Scheme} {token.access_token}");
            HttpResponseMessage rsMsg = _client.GetAsync(rsUrl).Result;

            Console.WriteLine("result of requesting the resouce server");
            Console.WriteLine(rsMsg.StatusCode);
            Console.WriteLine(rsMsg.Content.ReadAsStringAsync().Result);
        }
    }

    public class Token
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public double expires_in { get; set; }
    }

    public class ResponseData
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}