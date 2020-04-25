using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class ExtraFieldCategory
    {
        public int CategoryId { get; set; }
        public int ExtraFieldId { get; set; }
        [ForeignKey("ExtraFieldId")]
        public ExtraField ExtraField { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
