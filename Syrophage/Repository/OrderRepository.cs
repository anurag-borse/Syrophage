using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {

        public readonly ApplicationDbContext db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {

            this.db = db;
        }

        public Order GetById(int Id)
        {
            return db.Orders.Find(Id);
        }

        public List<Order> GetByUserID(int id)
        {
            return db.Orders.Where(e => e.UserId == id).ToList();
        }


        public void Update(Order obj)
        {
            db.Orders.Update(obj);
        }
    }
}