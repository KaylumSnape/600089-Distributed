using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DistSysAcwClient
{
    #region Task 10 and beyond
    class Client
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. What would you like to do?");
            string inputString = Console.ReadLine().ToLower();
            switch (inputString)
            {
                case string a when a.Contains("hello"):
                {
                    TalkBackHello().Wait();
                } break;
                default: { } break;
            }
            Console.ReadKey();
        }
        
        static async Task TalkBackHello()
        {
            client.BaseAddress = new Uri("http://localhost:44394/");
            try
            {
                var result = await client.GetAsync("api/talkback/hello");
                Console.WriteLine(result.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        static async Task<string> GetStringAsync(string path)
        {
            string responsestring = "";
            HttpResponseMessage response = await client.GetAsync(path);
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }

    }
    #endregion
}