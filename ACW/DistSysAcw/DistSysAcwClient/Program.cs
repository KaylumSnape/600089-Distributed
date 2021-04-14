using DistSysAcwClient.Class;
using System;

namespace DistSysAcwClient
{
    #region Task 10 and beyond
    internal class Client
    {
        private static void Main()
        {
            Console.WriteLine("Hello. What would you like to do?");

            var userInput = Console.ReadLine();

            while (userInput?.ToLower() != "exit")
            {
                try
                {
                    var splitInput = userInput?.Split(' ');

                    switch (splitInput) // Originally userInput
                    {
                        case { } a when a[0].ToLower() == "talkback" && a[1].ToLower() == "hello":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.TalkBackHello().Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "talkback" && a[1].ToLower() == "sort":
                            {
                                var integers = splitInput[2].Replace("[", "&integers=")
                                    .Replace("]", "")
                                    .Replace(",", "&integers=");

                                Console.WriteLine("...please wait...");
                                Tasks.TalkBackSort(integers).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "get":
                            {
                                Console.WriteLine("...please wait...");
                                if (splitInput.Length != 3) // Added this functionality so client response is same as server.
                                {
                                    throw new Exception("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
                                }
                                Tasks.UserGet(splitInput[2]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "post":
                            {
                                Console.WriteLine("...please wait...");
                                if (splitInput.Length != 3)
                                {
                                    throw new Exception("Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
                                }
                                Tasks.UserPost(splitInput[2]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "set":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.UserSet(splitInput[2], splitInput[3]);
                            }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "delete":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.UserDelete().Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "role":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.ChangeUserRole(splitInput[2], splitInput[3]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "hello":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.ProtectedHello().Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sha1":
                            {
                                Console.WriteLine("...please wait...");
                                if (splitInput.Length != 3)
                                {
                                    throw new Exception("Bad Request");
                                }
                                Tasks.ProtectedSha1(splitInput[2]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sha256":
                            {
                                Console.WriteLine("...please wait...");
                                if (splitInput.Length != 3)
                                {
                                    throw new Exception("Bad Request");
                                }
                                Tasks.ProtectedSha256(splitInput[2]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "get" && a[2].ToLower() == "publickey":
                            {
                                Console.WriteLine("...please wait...");
                                Tasks.GetPublicKey().Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sign":
                            {
                                Console.WriteLine("...please wait...");
                                if (splitInput.Length != 3)
                                {
                                    throw new Exception("Please enter a message to be signed.");
                                }
                                Tasks.ProtectedSign(splitInput[2]).Wait();
                            }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "addfifty":
                        {
                            Console.WriteLine("...please wait...");
                            if (splitInput.Length != 3)
                            {
                                throw new Exception("Please enter a message to be signed.");
                            }
                            
                            if (!int.TryParse(splitInput[2], out var o))
                            {
                                throw new Exception("A valid integer must be given!");
                            }

                            Tasks.ProtectedAddFifty(splitInput[2]).Wait();
                        }
                            break;

                        default:
                            {
                                Console.WriteLine("Command not recognised.");
                            }
                            break;
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