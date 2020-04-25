using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string MainImage { get; set; }
        public bool IsActive { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string NationalCode { get; set; }
        public int? DiscountId { get; set; }
        public Stock Stock { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<Gallery> Gallery { get; set; }
        public ICollection<ProductExtraField> ProductExtraFields { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }

    }

    public enum Stock
    {
        [Display(Name = "بزودی")]
        Soon = 1,
        [Display(Name = "ناموجود")]
        Unavailable = 2,
        [Display(Name = "موجود-نامحدود")]
        AvailableUnlimited = 3,
        [Display(Name = "موجود-محدود")]
        AvailableLimited = 4
    }
}
