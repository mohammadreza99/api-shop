using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Domain.Product
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public FeatureType Type { get; set; }
        public ICollection<FeatureValue> Values { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
    }

    public enum FeatureType
    {
        [Display(Name = "لیست")]
        List = 1,
        [Display(Name = "انتخاب رنگ")]
        ColorPicker = 2
    }
}
