using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DistSysAcwClient.Class
{
    internal class Tasks
    {
        private static readonly HttpClient Client = new HttpClient();
        
        private const string BaseDomain = "https://localhost:44394/";
        //private const string BaseDomain = "http://distsysacwserver.net.dcs.hull.ac.uk/2839013/";

        // http://distsysacwserver.net.dcs.hull.ac.uk/2839013/Api/Other/Clear

        public static string UserName = string.Empty;
        public static string ApiKey = "8cd9a4d8-a93c-4096-92e6-15c5b7ce25eb"; // "8cd9a4d8-a93c-4096-92e6-15c5b7ce25eb"
        public static string PublicKey = string.Empty;

        internal static async Task<string> TalkBackHello()
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/talkback/hello"),
                Method = HttpMethod.Get
            };
            var httpResponse = await Client.SendAsync(httpRequest);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            return responseString;
        }

        internal static async Task<int[]> TalkBackSort(string integers)
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/talkback/sort?{integers}"),
                Method = HttpMethod.Get
            };
            var httpResponse = await Client.SendAsync(httpRequest);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            var integerArray = Array.ConvertAll(responseString
                .Replace("[", "")
                .Replace("]", "")
                .Split(','), int.Parse);
            return integerArray;
        }

        internal static async Task<string> UserGet(string userName)
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/user/new?username={userName}"),
                Method = HttpMethod.Get
            };
            var httpResponse = await Client.SendAsync(httpRequest);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            return responseString;
        }

        internal static async Task<string> UserPost(string token)
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/user/new"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json")
            };
            var httpResponse = await Client.SendAsync(httpRequest);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                UserName = token;
                ApiKey = responseString;
                Console.WriteLine("Got API Key");
            }
            else
            {
                Console.WriteLine(responseString);
            }
            return responseString;
        }

        internal static void UserSet(string userName, string apiKey)
        {
            UserName = userName;
            ApiKey = apiKey;
            Console.WriteLine("Stored");
        }

        internal static async Task<bool> UserDelete()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return false;
            }

            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/user/removeuser?username={UserName}"),
                Method = HttpMethod.Delete
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
            if (httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(responseString);
                return true;
            }
            else
            {
                Console.WriteLine(false);
                return false;
            }
        }

        internal static async Task ChangeUserRole(string userName, string role)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var changeRole = new ChangeRole(userName, role);

            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/user/changerole"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(changeRole), Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            var response = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(response);
        }

        internal static async Task ProtectedHello()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }
            
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/hello"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            var response = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(response);
        }

        internal static async Task ProtectedSha1(string message)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }
            
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/sha1?message={message}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            var response = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(response);
        }

        internal static async Task ProtectedSha256(string message)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }
            
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/sha256?message={message}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            var response = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(response);
        }

        internal static async Task<string> GetPublicKey()
        {
            var response = "You need to do a User Post or User Set first";
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine(response);
                return response;
            }
            
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/getpublickey"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);
            var httpResponse = await Client.SendAsync(httpRequest);
            PublicKey = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                response = "Got Public Key";
                Console.WriteLine(response);
                return response;
            }
            else
            {
                response = "Couldn’t Get the Public Key";
                Console.WriteLine(response);
                return response;
            }
        }
    }
}
