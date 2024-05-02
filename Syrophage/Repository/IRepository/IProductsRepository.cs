using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IProductsRepository: IRepository<Product>
    {

        Product GetById(int Id);

        void Update(Product obj);

        List<Product> GetByCategoryName(String name);





    }
}