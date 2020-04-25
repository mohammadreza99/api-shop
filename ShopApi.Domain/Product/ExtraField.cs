using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class ExtraField
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Label { get; set; }
        public ExtraFieldType Type { get; set; }
        [ForeignKey("ParentId")]
        public virtual ExtraFieldGroup ExtraFieldGroup { get; set; }
        public ICollection<ExtraFieldValue> ExtraFieldValues { get; set; }
        public ICollection<ExtraFieldCategory> ExtraFieldCategories { get; set; }
        public ICollection<ProductExtraField> ProductExtraFields { get; set; }
    }

    public enum ExtraFieldType
    {
        [Display(Name = "لیست")]
        List = 1,
        [Display(Name = "متن")]
        Text = 2,
        [Display(Name = "بلی/خیر")]
        Boolean = 3
    }
}
