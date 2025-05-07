// GoTorz/Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GoTorz.Hubs
{
    // [Authorize] // <<< KEPT COMMENTED OUT
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message) // Expects only message
        {
            var userName = Context.User?.Identity?.IsAuthenticated == true ? Context.User.Identity.Name : "Guest_Hub";
            userName = string.IsNullOrEmpty(userName) ? "Guest_Hub_Fallback" : userName;

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("ChatHub: Received empty message, not broadcasting.");
                return;
            }
            var timestamp = DateTime.UtcNow; // Server adds timestamp

            Console.WriteLine($"ChatHub: Broadcasting message from '{userName}': '{message}' with timestamp {timestamp}");
            // Client in the above GlobalChatWidget.razor expects user, message, timestamp
            await Clients.All.SendAsync("ReceiveMessage", userName, message, timestamp);
        }
        // OnConnectedAsync and OnDisconnectedAsync can remain as they were
        public override Task OnConnectedAsync()
        {
            var userNameForLog = Context.User?.Identity?.IsAuthenticated == true ? Context.User.Identity.Name : "Anonymous";
            userNameForLog = string.IsNullOrEmpty(userNameForLog) ? "Anonymous_Fallback" : userNameForLog;
            Console.WriteLine($"ChatHub: Client connected. ConnectionId: {Context.ConnectionId}, User: {userNameForLog}");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userNameForLog = Context.User?.Identity?.IsAuthenticated == true ? Context.User.Identity.Name : "Anonymous";
            userNameForLog = string.IsNullOrEmpty(userNameForLog) ? "Anonymous_Fallback" : userNameForLog;
            Console.WriteLine($"ChatHub: Client disconnected. ConnectionId: {Context.ConnectionId}, User: {userNameForLog}, Exception: {exception?.Message}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}