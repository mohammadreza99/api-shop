using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
        public int ProductId { get; set; }
        public DateTime ExpireDate { get; set; }
        public DiscountType Type { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public enum DiscountType
    {
        [Display(Name ="بدون تخفیف")]
        None = 0,
        [Display(Name = "تخفیف ثابت")]
        Fix = 1,
        [Display(Name = "تخفیف درصدی")]
        Percent = 2
    }
}
