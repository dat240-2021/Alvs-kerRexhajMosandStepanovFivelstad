using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using backend.Core.Domain.BackendGame.Models;
using backend.Core.Domain.BackendGame.Pipelines;

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
