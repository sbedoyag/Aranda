using Aranda.Core.Entities;
using Aranda.Core.Interfaces;

namespace Aranda.EntityFramework.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductContext context) 
            : base(context)
        {
        }
    }
}
