using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRLab.Hubs
{
    // Get the port number from launchSettings.json.
    // http://localhost:58008
    public class ChatHub : Hub
    {
        /* Clients can remotely call this endpoint.
         Receives a username and a message then sends the username and message to all connected clients.
         It is asynchronous, which means that the rest of our program can continue to function
         whilst our message is sent out to connected clients by the underlying framework.
         */
        public async Task BroadcastMessage(string username, string message)
        {
            // String method - Identifies the remote function that we will be calling on the client.
            await Clients.All.SendAsync("GetMessage", username, message);
        }
    }
}