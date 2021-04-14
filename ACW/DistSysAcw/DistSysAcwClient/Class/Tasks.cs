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
        //private const string BaseDomain = "http://150.237.94.9/2839013/"; // http://distsysacwserver.net.dcs.hull.ac.uk/2839013/Api/Other/Clear

        public static string UserName = string.Empty;
        public static string ApiKey = "8cd9a4d8-a93c-4096-92e6-15c5b7ce25eb"; // 
        //public static string ApiKey = "e8c0c77e-ee70-4e41-a840-367aae696ec6"; // Test Server.
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

        internal static async Task<string> ProtectedSign(string message)
        {
            var response = "You need to do a User Post or User Set first";
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine(response);
                return response;
            }
            if (string.IsNullOrWhiteSpace(PublicKey))
            {
                response = "Client doesn’t yet have the public key";
                Console.WriteLine(response);
                return response;
            }

            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/sign?message={message}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);

            var httpResponse = await Client.SendAsync(httpRequest);
            if (httpResponse.IsSuccessStatusCode)
            {
                var signedHexMessage = await httpResponse.Content.ReadAsStringAsync();

                var originalMessageBytes = Encoding.ASCII.GetBytes(message);
                var signedBytes = Converters.HexStringToBytes(signedHexMessage);

                var verified = RsaCsp.Verify(PublicKey, originalMessageBytes, signedBytes);

                if (verified)
                {
                    response = "Message was successfully signed";
                    Console.WriteLine(response);
                    return response;
                }
            }

            response = "Message was not successfully signed";
            Console.WriteLine(response);
            return response;
        }

        internal static async Task<string> ProtectedAddFifty(string stringInt)
        {
            var response = "You need to do a User Post or User Set first";
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                Console.WriteLine(response);
                return response;
            }
            if (string.IsNullOrWhiteSpace(PublicKey))
            {
                response = "Client doesn’t yet have the public key";
                Console.WriteLine(response);
                return response;
            }
            
            // Get newly generated Aes info (key and IV) and int as bytes, add to a list.
            var bytesToEncrypt = AesProvider.GetAesInfo();
            bytesToEncrypt.Add(Encoding.ASCII.GetBytes(stringInt));

            // Encrypt the data in the list with RSA public key from server.
            var encryptedList = RsaCsp.EncryptList(PublicKey, bytesToEncrypt);

            // Get the string of the encrypted data and assign to respective values.
            var encryptedHexSymKey = BitConverter.ToString(encryptedList[0]);
            var encryptedHexIv = BitConverter.ToString(encryptedList[1]);
            var encryptedHexInteger = BitConverter.ToString(encryptedList[2]);
            
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseDomain}api/protected/addfifty?encryptedInteger={encryptedHexInteger}&encryptedSymKey={encryptedHexSymKey}&encryptedIV={encryptedHexIv}"),
                Method = HttpMethod.Get
            };
            httpRequest.Headers.Add("ApiKey", ApiKey);

            var httpResponse = await Client.SendAsync(httpRequest);
            if (httpResponse.IsSuccessStatusCode)
            {
                var aesEncryptedHexInteger = await httpResponse.Content.ReadAsStringAsync();
                
                var aesEncryptedInteger = Converters.HexStringToBytes(aesEncryptedHexInteger);

                var decryptedInteger = AesProvider.Decrypt(bytesToEncrypt[0], bytesToEncrypt[1], aesEncryptedInteger);

                if (decryptedInteger != null)
                {
                    response = decryptedInteger;
                    Console.WriteLine(response);
                    return response;
                }
            }

            response = "An error occurred!";
            Console.WriteLine(response);
            return response;
        }
    }
}
