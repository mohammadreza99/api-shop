using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Domain.Product
{
    public class ProductFeature
    {
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        public int FeatureValueId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }
        [ForeignKey("FeatureValueId")]
        public virtual FeatureValue FeatureValue { get; set; }
    }
}
