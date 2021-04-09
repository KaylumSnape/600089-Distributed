using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
namespace DistSysACWClient.Class
{
    class Tasks
    {
        static HttpClient client = new HttpClient();
        public static string Data_verify;
        public static dynamic pKey;
        // UserClass user = new UserClass();
        public static string api_key = "";
        //-----------------------------------------------------------------------TALKBACK HELLO---------------------------------------------------------------------------//
        public static async Task<string> TalkbackHello()
        {
           // client.BaseAddress = new Uri("https://localhost:44307/");
                HttpRequestMessage httpRequest = new HttpRequestMessage();
                httpRequest.RequestUri =new Uri("https://localhost:44307/api/talkback/hello");
                httpRequest.Method = HttpMethod.Get;
                httpRequest.Headers.Add("apikey", return_api());       //for authorization
                HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
                string resp = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(resp);
                return resp;
        }
        
        //-----------------------------TALKBACKSORT METHOD-------------------------------------------------------------//
        public static async Task<string> TalkbackSort(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/talkback/sort?num=" + tst);
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());        //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;

        }
        //-----------------------------GETUSR METHOD-------------------------------------------------------------//
        public static async Task<string> TalkbackGetUsr(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/user/new?name=" + tst);
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());   //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;
        }
        //--------------------------POSTUSR------------------------------------------------------------------//
        public static async Task<string> TalkbackPostGetUsr(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/user/new");
            httpRequest.Method = HttpMethod.Post;
            var stringContent = new StringContent(JsonConvert.SerializeObject(tst), Encoding.UTF8, "application/json");   //conversion to json string
            httpRequest.Content = stringContent;
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            api_key = resp;
            Console.WriteLine(resp);
            return resp;

        }
        //-------------------------DELETEUSER-------------------------------------------------------------------//
        public static async Task<string>TalkbackDeleteUser(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/user/DeleteUser?name="+tst);
            httpRequest.Method = HttpMethod.Delete;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;
        }
        //------------------------------------------PROTECTEDHELLO-------------------------------------------------------------------//
        public static async Task<string> TalkbackProtectedHello()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/protected/hello");
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;
        }
        //------------------------------------protectedsha1-----------------------------------------------------------------------------------//
        public static async Task<string> TalkbackProtectedSHA1(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/protected/sha1?message="+tst);
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;
        }
        //----------------------------------------PROTECTED256------------------------------------------------------------------------------------//
        public static async Task<string> TalkbackProtectedSHA256(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/protected/sha256?message=" + tst);
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            return resp;
        }
        //-----------------------------------------RETURNPUBLICKEY------------------------------------------------------------------------------//
        public static async Task<string> TalkbackProtectedPublic_key()
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/protected/GetPublicey");
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            pKey = resp;
            return resp;
        }
        //------------------------------------------SIGHWITHPRIVATEKEY------------------------------------------//
        public static async Task<string> TalkbackProtectedPrivate_key(string tst)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri("https://localhost:44307/api/protected/sign?message="+tst);
            httpRequest.Method = HttpMethod.Get;
            httpRequest.Headers.Add("apikey", return_api());      //for authorization
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            string resp = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
            Data_verify = resp;
            return resp;
        }
        //----------------------------------------JUST RETURNS API--------------------------------------------------------------------//
        public static string return_api()
        {
            return api_key;
        }
    }
}
