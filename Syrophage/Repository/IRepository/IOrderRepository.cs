using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IOrderRepository :IRepository<Order>
    {


        Order GetById(int Id); 
        
        void Update(Order obj);

        List<Order> GetByUserID(int id);





    }
}