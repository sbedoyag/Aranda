using Aranda.Core.Interfaces;
using Aranda.EntityFramework.Repositories;

namespace Aranda.EntityFramework.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _context;
        public IProductRepository Products { get; private set; }

        public UnitOfWork(ProductContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
        }
        
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
