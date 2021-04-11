using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DistSysAcwClient.Class;
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

            var userInput = Console.ReadLine();
            
            while (userInput != "exit")
            {
                try
                {
                    switch (userInput)
                    {
                        case {} a when a.ToLower().Contains("talkback") && a.ToLower().Contains("hello"):
                        {
                            Tasks.TalkBackHello().Wait();
                        } break;

                        case {} a when a.ToLower().Contains("talkback") && a.ToLower().Contains("sort"):
                        {
                            var split = userInput.Split('[');
                            var tokens = split[1];
                            Tasks.TalkBackSort(tokens.Replace(",", "&integers=")
                                .Replace("]", "")).Wait();
                        } break;

                        case {} a when a.ToLower().Contains("user") && a.ToLower().Contains("get"):
                        {
                            var split = userInput.Split(' ');
                            Tasks.UserGet(split[2]).Wait();
                        } break;

                        case {} a when a.ToLower().Contains("user") && a.ToLower().Contains("post"):
                        {
                            var split = userInput.Split(' ');
                            Tasks.UserPost(split[2]).Wait();
                        } break;

                        case {} a when a.ToLower().Contains("user") && a.ToLower().Contains("set"):
                        {
                            var split = userInput.Split(' ');
                            Tasks.UserSet(split[2], split[3]);
                        } break;

                        case {} a when a.ToLower().Contains("user") && a.ToLower().Contains("delete"):
                        {
                            var split = userInput.Split(' ');
                            Tasks.UserDelete(split[1]).Wait();
                        } break;

                        default: { } break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.GetBaseException().Message);
                }

                Console.WriteLine("What would you like to do next?");
                userInput = Console.ReadLine();
                Console.Clear();
            }
            Environment.Exit(1);
        }
    }
    #endregion
}