using System;
using System.Text;
using DistSysAcwClient.Class;

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
                var response = string.Empty;
                try
                {
                    var splitInput = userInput?.Split(' ');

                    switch (splitInput)
                    {
                        case { } a when a[0].ToLower() == "talkback" && a[1].ToLower() == "hello":
                        {
                            Console.WriteLine("...please wait...");
                            response = Tasks.TalkBackHello().Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "talkback" && a[1].ToLower() == "sort":
                        {
                            if (splitInput.Length != 3)
                            {
                                response = "Please enter a array of integers to sort, e.g. [5,4,2,1]";
                                break;
                            }

                            var integers = splitInput[2].Replace("[", "&integers=")
                                .Replace("]", "")
                                .Replace(",", "&integers=");

                            Console.WriteLine("...please wait...");
                            Tasks.TalkBackSort(integers).Wait();
                        }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "get":
                        {
                            if (splitInput.Length != 3)
                            {
                                response = "Please enter a username.";
                                break;
                            }

                            Console.WriteLine("...please wait...");
                            response = Tasks.UserGet(splitInput[2]).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "post":
                        {
                            if (splitInput.Length != 3)
                            {
                                response = "Please enter a username.";
                                break;
                            }

                            Console.WriteLine("...please wait...");
                            response = Tasks.UserPost(splitInput[2]).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "set":
                        {
                            if (splitInput.Length != 4)
                            {
                                response = "Please enter a username and api key.";
                                break;
                            }

                            Console.WriteLine("...please wait...");
                            Tasks.UserSet(splitInput[2], splitInput[3]);
                            response = "Stored";
                        }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "delete":
                        {
                            Console.WriteLine("...please wait...");
                            response = Tasks.UserDelete().Result.ToString();
                        }
                            break;

                        case { } a when a[0].ToLower() == "user" && a[1].ToLower() == "role":
                        {
                            if (splitInput.Length != 4)
                            {
                                response = "Please enter a username and role.";
                                break;
                            }

                            Console.WriteLine("...please wait...");
                            response = Tasks.ChangeUserRole(splitInput[2], splitInput[3]).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "hello":
                        {
                            Console.WriteLine("...please wait...");
                            response = Tasks.ProtectedHello().Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sha1":
                        {
                            if (splitInput.Length < 3)
                            {
                                response = "Please provide a message.";
                                break;
                            }

                            // What happens when the message contain spaces!!
                            // Will the message contain spaces?
                            // Do the spaces need to be escaped?!
                            var message = new StringBuilder();
                            for (var i = 2; i < splitInput.Length; i++) message.Append(splitInput[i]).Append(" ");

                            Console.WriteLine("...please wait...");
                            response = Tasks.ProtectedSha1(message.ToString().Trim()).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sha256":
                        {
                            if (splitInput.Length < 3)
                            {
                                response = "Please provide a message.";
                                break;
                            }

                            var message = new StringBuilder();
                            for (var i = 2; i < splitInput.Length; i++) message.Append(splitInput[i]).Append(" ");

                            Console.WriteLine("...please wait...");
                            response = Tasks.ProtectedSha256(message.ToString().Trim()).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "get" &&
                                        a[2].ToLower() == "publickey":
                        {
                            Console.WriteLine("...please wait...");
                            response = Tasks.GetPublicKey().Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "sign":
                        {
                            if (splitInput.Length < 3)
                            {
                                response = "Please provide a message.";
                                break;
                            }

                            var message = new StringBuilder();
                            for (var i = 2; i < splitInput.Length; i++) message.Append(splitInput[i]).Append(" ");

                            Console.WriteLine("...please wait...");
                            response = Tasks.ProtectedSign(message.ToString().Trim()).Result;
                        }
                            break;

                        case { } a when a[0].ToLower() == "protected" && a[1].ToLower() == "addfifty":
                        {
                            if (splitInput.Length != 3)
                            {
                                response = "Please provide a message.";
                                break;
                            }

                            if (!int.TryParse(splitInput[2], out var o))
                            {
                                response = "A valid integer must be given!";
                                break;
                            }

                            Console.WriteLine("...please wait...");
                            response = Tasks.ProtectedAddFifty(splitInput[2]).Result;
                        }
                            break;

                        default:
                        {
                            Console.WriteLine("Command not recognised.");
                        }
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(response)) break;
                    Console.WriteLine(response);
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