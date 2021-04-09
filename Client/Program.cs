using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:22288/");
            try
            {
                Task<string> task = GetStringAsync("/translate/GetString/hello");
                if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                { Console.WriteLine(task.Result); }
                else
                { Console.WriteLine("Request Timed Out"); }
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
}