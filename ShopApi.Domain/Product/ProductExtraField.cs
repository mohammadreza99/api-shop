using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class ProductExtraField
    {
        public int ProductId { get; set; }
        public int ExtraFieldId { get; set; }
        public int ExtraFieldValueId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("ExtraFieldId")]
        public virtual ExtraField ExtraField { get; set; }
        [ForeignKey("ExtraFieldValueId")]
        public virtual ExtraFieldValue ExtraFieldValue { get; set; }
    }
}
