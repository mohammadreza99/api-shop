using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }
        public ICollection<Category> Children { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ExtraFieldCategory> ExtraFieldCategories { get; set; }
    }
}
