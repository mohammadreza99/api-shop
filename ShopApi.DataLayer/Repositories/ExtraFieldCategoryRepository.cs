using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.DataLayer.Data;

namespace ShopApi.DataLayer.Repositories
{
    public class ExtraFieldCategoryRepository
    {
        private readonly DatabaseContext _context;

        public ExtraFieldCategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable<ExtraFieldCategory> Get()
        {
            return _context.Set<ExtraFieldCategory>();
        }

        public ExtraFieldCategory GetByIds(int extraFieldId, int categoryId)
        {
            return _context.Set<ExtraFieldCategory>().FirstOrDefault(obj => obj.CategoryId == categoryId && obj.ExtraFieldId == extraFieldId);
        }

        public void Add(ExtraFieldCategory extraFieldCategory)
        {
            _context.Add(extraFieldCategory);
        }

        public void Update(ExtraFieldCategory extraFieldCategory)
        {
            _context.Attach(extraFieldCategory);
            _context.Entry(extraFieldCategory).State = EntityState.Modified;
        }

        public void Delete(ExtraFieldCategory extraFieldCategory)
        {
            if (_context.Entry(extraFieldCategory).State == EntityState.Detached) 
                _context.Attach(extraFieldCategory);
            _context.Remove(extraFieldCategory);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
