using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        private readonly ApplicationDbContext _db;

        public TokenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

    }
}
