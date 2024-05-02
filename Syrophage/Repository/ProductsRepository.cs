using Syrophage.Data;
using Syrophage.Models;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    internal class ProductsRepository : Repository<Product>, IProductsRepository
    {

        public readonly ApplicationDbContext db;
        public ProductsRepository(ApplicationDbContext db) : base(db)
        {

            this.db = db;
        }

        public List<Product> GetByCategoryName(string name)
        {
            return db.Products.Where(e => e.Category == name).ToList();
        }

        public Product GetById(int Id)
        {
            return db.Products.Find(Id);
        }

        public void Update(Product obj)
        {
            db.Products.Update(obj);
        }
    }
}