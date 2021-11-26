using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.Lobby.Models;

namespace backend.Hubs
{
    public class LobbyHub : Hub
    {
        public async Task SendMessage(Game game)
        {
            await Clients.All.SendAsync("GameCreated", game);
        }
    }
}
