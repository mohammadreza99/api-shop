using System;
using System.Collections.Generic;
using ShopApi.Domain.Product;

namespace ShopApi.DataLayer.DataStructure
{
    public class ProductDs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
        public string MainImage { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string Stock { get; set; }
        public string NationalCode { get; set; }
        public int CategoryId { get; set; }
        public BrandDs Brand { get; set; }
        public DiscountDs Discount { get; set; }
        public ICollection<TagDs> Tags { get; set; }
        public ICollection<GalleryDs> Gallery { get; set; }
        public ICollection<ProductFeatureDs> Features { get; set; }
        public ICollection<ProductExtraFieldDs> ExtraFields { get; set; }
    }

    public class CategoryDs : BaseItem
    {
        public int? ParentId { get; set; }
        public List<int> ExtraFieldIds { get; set; }
    }

    public class GalleryDs
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }

    public class ExtraFieldDs : BaseItem
    {
        public string Type { get; set; }
        public ExtraFieldGroupDs Group { get; set; }
        public ICollection<int> CategoryIds { get; set; }
        public ICollection<string> ListItems { get; set; }
    }

    public class ProductExtraFieldDs
    {
        public ExtraFieldDs ExtraField { get; set; }
        public BaseItem Value { get; set; }
    }

    public class FeatureDs : BaseItem
    {
        public string Type { get; set; }
        public ICollection<FeatureValueDs> Values { get; set; }
    }

    public class ProductFeatureDs
    {
        public FeatureDs Feature { get; set; }
        public BaseItem Value { get; set; }
    }

    public class FeatureValueDs:BaseItem
    {
        public int FeatureId { get; set; }
        public string ColorCode { get; set; }
    }

    public class DiscountDs
    {
        public int? Id { get; set; }
        public int Value { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Type { get; set; }
    }

    public class TagDs : BaseItem { }

    public class ExtraFieldGroupDs : BaseItem { }

    public class BrandDs : BaseItem { }

    public class Tree
    {
#nullable enable
        public string Label { get; set; }
        public object? Data { get; set; }
        public ICollection<Tree> Children { get; set; }
        public object? Icon { get; set; }
        public object? ExpandedIcon { get; set; }
        public object? CollapsedIcon { get; set; }
        public bool? Leaf { get; set; }
        public bool? Expanded { get; set; }
    }

    public class TreeTable
    {
        public object? Data { get; set; }
        public List<TreeTable> Children { get; set; }
        public object? Icon { get; set; }
        public object? ExpandedIcon { get; set; }
        public object? CollapsedIcon { get; set; }
        public bool? Leaf { get; set; }
        public bool? Expanded { get; set; }
    }

    public class BaseItem
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
