using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DistSysAcwClient.Class
{
    internal class Tasks
    {
        private static readonly HttpClient Client = new HttpClient();

        //  http://distsysacwserver.net.dcs.hull.ac.uk/2839013/
        private const string BaseDomain = "https://localhost:44394/";

        public static string UserName = string.Empty;
        public static string ApiKey = string.Empty;

        internal static async Task TalkBackHello()
        {
            try
            {
                Console.WriteLine("...please wait...");
                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{BaseDomain}api/talkback/hello"),
                    Method = HttpMethod.Get
                };
                var httpResponse = await Client.SendAsync(httpRequest);
                Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        internal static async Task TalkBackSort(string tokens)
        {
            try
            {
                Console.WriteLine("...please wait...");
                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{BaseDomain}api/talkback/sort?integers={tokens}"),
                    Method = HttpMethod.Get
                };
                var httpResponse = await Client.SendAsync(httpRequest);
                Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        internal static async Task UserGet(string token)
        {
            try
            {
                Console.WriteLine("...please wait...");
                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{BaseDomain}api/user/new?username={token}"), 
                    Method = HttpMethod.Get
                };
                var httpResponse = await Client.SendAsync(httpRequest);
                Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        internal static async Task UserPost(string token)
        {
            try
            {
                Console.WriteLine("...please wait...");
                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{BaseDomain}api/user/new"),
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json")
                };
                var httpResponse = await Client.SendAsync(httpRequest);
                var response = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    UserName = token;
                    ApiKey = response;
                    Console.WriteLine("Got API Key");
                }
                else
                {
                    Console.WriteLine(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        internal static void UserSet(string userName, string apiKey)
        {
            try
            {
                Console.WriteLine("...please wait...");

                UserName = userName;
                ApiKey = apiKey;

                Console.WriteLine("Stored");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        internal static async Task UserDelete(string token)
        {
            try
            {
                Console.WriteLine("...please wait...");

                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(ApiKey))
                {
                    Console.WriteLine("You need to do a User Post or User Set first");
                    return;
                }

                var httpRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(BaseDomain + $"api/user/removeuser?username={UserName}"),
                    Method = HttpMethod.Delete
                };
                httpRequest.Headers.Add("ApiKey", ApiKey);
                var httpResponse = await Client.SendAsync(httpRequest);
                var response = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine(response);
                }
                else
                {
                    Console.WriteLine(false);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }
    }
}
