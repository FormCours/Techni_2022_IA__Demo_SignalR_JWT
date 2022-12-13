using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Demo_ASP__SignalR_JWT.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {

        public async Task SendMessage(string message)
        {
            string pseudo = Context.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "N/A";

            await Clients.All.SendAsync("receiveMessage", pseudo, message);
        }
    }
}
