using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.DataLayer.Data;

namespace ShopApi.DataLayer.Repositories
{
    public class ProductTagRepository
    {
        private readonly DatabaseContext _context;

        public ProductTagRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable<ProductTag> Get()
        {
            return _context.Set<ProductTag>();
        }

        public ProductTag GetByIds(int productId, int tagId)
        {
            return _context.Set<ProductTag>().FirstOrDefault(obj => obj.ProductId == productId && obj.TagId == tagId);
        }

        public void Add(ProductTag productTag)
        {
            _context.Add(productTag);
        }

        public void Update(ProductTag productTag)
        {
            _context.Attach(productTag);
            _context.Entry(productTag).State = EntityState.Modified;
        }

        public void Delete(ProductTag productTag)
        {
            if (_context.Entry(productTag).State == EntityState.Detached)
                _context.Attach(productTag);
            _context.Remove(productTag);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
