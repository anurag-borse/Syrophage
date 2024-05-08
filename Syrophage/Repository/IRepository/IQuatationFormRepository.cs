using Microsoft.EntityFrameworkCore;
using Syrophage.Models;

namespace Syrophage.Repository.IRepository
{
    public interface IQuatationFormRepository: IRepository<QuotationFormData>
    {
        QuotationFormData GetById(int Id);

        void Update(QuotationFormData obj);
        public IQueryable<QuotationFormData> GetAlll();
        

    }
}