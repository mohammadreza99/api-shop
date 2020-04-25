using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class ExtraFieldValue
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public int ExtraFieldId { get; set; }
        [ForeignKey("ExtraFieldId")]
        public virtual ExtraField ExtraField { get; set; }
    }
}
