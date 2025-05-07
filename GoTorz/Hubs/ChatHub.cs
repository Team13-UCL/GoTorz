using Microsoft.AspNetCore.SignalR;
using System; // For DateTime
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // To restrict to authenticated users

namespace GoTorz.Hubs
{
    // [Authorize]
    public class ChatHub : Hub
    {
        // Client calls this method to send a message
        public async Task SendMessage(string message)
        {
            // Get the name of the calling user.
            // Context.User.Identity.Name should be populated if using cookie authentication correctly.
            // This relies on the ClaimTypes.Name claim being set during login.
            var userName = Context.User?.Identity?.Name;

            // Fallback if the name claim wasn't found, though [Authorize] should ensure an authenticated user.
            if (string.IsNullOrEmpty(userName))
            {
                // You might want to log this situation if it occurs despite [Authorize]
                userName = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Authenticated User (Name not found)";
            }

            // Additional check, though [Authorize] on the Hub should make this redundant.
            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                // Should not be hit if [Authorize] works correctly.
                // If it is, do not proceed.
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return; // Don't send empty messages
            }

            var timestamp = DateTime.UtcNow; // Use UTC for server-side consistency

            // Broadcast the message to all connected clients
            // Clients will listen for "ReceiveMessage" and will get sender's name, message, and timestamp
            await Clients.All.SendAsync("ReceiveMessage", userName, message, timestamp);
        }

        public override async Task OnConnectedAsync()
        {
            var userNameForLog = Context.User?.Identity?.Name ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Unknown User";
            Console.WriteLine($"User connected to ChatHub: {userNameForLog} (ConnectionId: {Context.ConnectionId})");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userNameForLog = Context.User?.Identity?.Name ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Unknown User";
            Console.WriteLine($"User disconnected from ChatHub: {userNameForLog} (ConnectionId: {Context.ConnectionId}). Exception: {exception?.Message}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
