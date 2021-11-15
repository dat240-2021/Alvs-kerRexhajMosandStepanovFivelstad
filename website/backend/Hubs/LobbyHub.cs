using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;

namespace backend.Hubs
{
    public class GamesHub: Hub
    {
        public async Task SendMessage(Game game)
        {
            await Clients.All.SendAsync("GameCreated", game);
        }
    }
}
