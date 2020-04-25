using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class FeatureValue
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public string ColorCode { get; set; }
        public int FeatureId { get; set; }
        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
    }
}
