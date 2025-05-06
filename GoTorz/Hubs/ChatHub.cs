using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GoTorz.Hubs
{
   // [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            var user = Context.User?.Identity?.Name ?? "Anonymous";
            if (!string.IsNullOrWhiteSpace(message))
            {
                // Log at der sendes en besked
                Console.WriteLine($"--> ChatHub: User '{user}' sending message: '{message}'");
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Log FØR base kald og potentiel fejl
            var userName = Context.User?.Identity?.Name ?? "UNKNOWN_USER";
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"--> ChatHub: User connecting: '{userName}' (ConnectionId: {connectionId})");

            try
            {
                await base.OnConnectedAsync(); // Kald base FØR du bruger Clients

                // Log EFTER succesfuldt base kald, før Clients kald
                Console.WriteLine($"--> ChatHub: Base.OnConnectedAsync completed for {connectionId}. Attempting Clients.Others...");

                // Send "joined" besked til ANDRE
                await Clients.Others.SendAsync("ReceiveMessage", "System", $"{userName} has joined the chat.");

                // Log EFTER succesfuldt Clients kald
                Console.WriteLine($"--> ChatHub: 'User joined' message sent for {connectionId}. OnConnectedAsync fully completed.");
            }
            catch (Exception ex)
            {
                // Log HVIS der sker en fejl UNDER OnConnectedAsync
                Console.WriteLine($"!!! ChatHub: EXCEPTION in OnConnectedAsync for {connectionId}: {ex.ToString()}");
                // Overvej om forbindelsen skal lukkes manuelt her, eller om SignalR håndterer det
                // Context.Abort(); // Kan bruges til eksplicit at lukke forbindelsen ved fejl
                throw; // Vigtigt at kaste exception videre, så SignalR ved, at der skete en fejl
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User?.Identity?.Name ?? "UNKNOWN_USER";
            var connectionId = Context.ConnectionId;
            // Log altid disconnect, inkluder exception hvis den findes
            Console.WriteLine($"--> ChatHub: User disconnected: '{userName}' (ConnectionId: {connectionId}), Exception: {exception?.Message ?? "None"}");

            try
            {
                await base.OnDisconnectedAsync(exception); // Kald base FØR Clients

                Console.WriteLine($"--> ChatHub: Base.OnDisconnectedAsync completed for {connectionId}. Attempting Clients.Others...");

                // Send "left" besked til ANDRE
                await Clients.Others.SendAsync("ReceiveMessage", "System", $"{userName} has left the chat.");

                Console.WriteLine($"--> ChatHub: 'User left' message sent for {connectionId}. OnDisconnectedAsync fully completed.");
            }
            catch (Exception ex)
            {
                // Log HVIS der sker en fejl UNDER OnDisconnectedAsync
                Console.WriteLine($"!!! ChatHub: EXCEPTION in OnDisconnectedAsync for {connectionId}: {ex.ToString()}");
                throw; // Kast videre
            }
        }
    }
}