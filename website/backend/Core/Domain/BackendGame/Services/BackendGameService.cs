using Infrastructure.Data;

namespace backend.Core.Domain.BackendGame.Services
{
    public interface IBackendGameService
    {
        
    }

    public class BackendGameService: IBackendGameService
    {
        private GameContext _db;

        public BackendGameService(GameContext db)
        {
            _db = db;
        }
        
        
    }
}