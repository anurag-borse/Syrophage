using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface ITokenRepository : IRepository<Token>
    {
        Token GetById(int Id);
        void Update(Token obj);
    }
}
