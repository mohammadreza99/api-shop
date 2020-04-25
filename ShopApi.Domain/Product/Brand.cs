using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Domain.Product
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}