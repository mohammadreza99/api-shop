using System;
using System.Collections.Generic;
using System.Linq;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using ShopApi.Interfaces.Product;

namespace ShopApi.Services.Product
{
    public class ProductService : IProduct
    {
        private readonly CrudOperationDs _crudDs;
        private readonly GenericRepository<Domain.Product.Product> _productRepository;
        private readonly GenericRepository<ProductTag> _productTagRepository;
        private readonly GenericRepository<Gallery> _galleryRepository;
        private readonly GenericRepository<ProductExtraField> _productExtraFieldRepository;
        private readonly GenericRepository<ProductFeature> _productFeatureRepository;

        public ProductService(DatabaseContext context)
        {
            _crudDs = new CrudOperationDs();
            _productRepository = new GenericRepository<Domain.Product.Product>(context);
            _productTagRepository = new GenericRepository<ProductTag>(context);
            _galleryRepository = new GenericRepository<Gallery>(context);
            _productExtraFieldRepository = new GenericRepository<ProductExtraField>(context);
            _productFeatureRepository = new GenericRepository<ProductFeature>(context);
        }

        public CrudOperationDs GetAll()
        {
            var result = new List<ProductDs>();
            foreach (var product in _productRepository.Get().ToList())
            {
                result.Add(FillProduct(product));
            }
            _crudDs.Result = result;
            return _crudDs;
        }
        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var product = _productRepository.GetById(id);
                _crudDs.Result = FillProduct(product);
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }
        public CrudOperationDs Add(ProductDs product)
        {
            var newProduct = _productRepository.Add(new Domain.Product.Product()
            {
                Name = product.Name,
                Description = product.Description,
                CreateDate = product.CreateDate,
                UpdateDate = product.UpdateDate,
                ReleaseDate = product.ReleaseDate,
                IsActive = product.IsActive,
                MainImage = product.MainImage,
                Count = product.Count,
                Price = product.Price,
                NationalCode = product.NationalCode,
                CategoryId = product.CategoryId,
                BrandId = product.Brand.Id,
                DiscountId = product.Discount.Id,
            });
            _productRepository.Save();
            AddProductTags(product.Tags, newProduct.Id);
            AddProductFeatures(product.Features, newProduct.Id);
            AddProductGallery(product.Gallery, newProduct.Id);
            AddProductExtraFields(product.ExtraFields, newProduct.Id);
            _crudDs.Result = product;
            return _crudDs;
        }
        public CrudOperationDs Update(ProductDs product)
        {
            _productRepository.Update(new Domain.Product.Product
            {
                Name = product.Name,
                Description = product.Description,
                CreateDate = product.CreateDate,
                UpdateDate = product.UpdateDate,
                ReleaseDate = product.ReleaseDate,
                IsActive = product.IsActive,
                MainImage = product.MainImage,
                Count = product.Count,
                Price = product.Price,
                NationalCode = product.NationalCode,
                CategoryId = product.CategoryId,
                BrandId = product.Brand.Id,
                DiscountId = product.Discount.Id,
            });
            _productRepository.Save();
            UpdateProductTags(product);
            UpdateProductFeatures(product);
            UpdateProductGallery(product);
            UpdateProductExtraFields(product);
            _crudDs.Result = product;
            return _crudDs;
        }
        public CrudOperationDs Delete(object id)
        {
            var product = _productRepository.GetById(id);
            _productRepository.Delete(id);
            _productRepository.Save();
            _crudDs.Result = product;
            return _crudDs;
        }
        public CrudOperationDs GetStockTypes()
        {
            var result= new List<string>();
            foreach (var item in Enum.GetValues(typeof(Stock)))
            {
                result.Add(EnumExtension<Stock>.GetDisplayName((Stock)item));
            }
            _crudDs.Result = result;
            return _crudDs;
        }
        public CrudOperationDs GetDiscountTypes()
        {
            var result = new List<string>();
            foreach (var item in Enum.GetValues(typeof(Discount)))
            {
                result.Add(EnumExtension<Discount>.GetDisplayName((Discount)item));
            }
            _crudDs.Result = result;
            return _crudDs;
        }
        private ProductDs FillProduct(Domain.Product.Product product)
        {
            var productTags = new List<TagDs>();
            var productGallery = new List<GalleryDs>();
            var productExtraFields = new List<ProductExtraFieldDs>();
            var productFeatures = new List<ProductFeatureDs>();
            FillProductTags(product, ref productTags);
            FillProductFeatures(product, ref productFeatures);
            FillProductGallery(product, ref productGallery);
            FillProductExtraFields(product, ref productExtraFields);
            var productDs = new ProductDs()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CreateDate = product.CreateDate,
                UpdateDate = product.UpdateDate,
                ReleaseDate = product.ReleaseDate,
                IsActive = product.IsActive,
                MainImage = product.MainImage,
                Count = product.Count,
                Price = product.Price,
                NationalCode = product.NationalCode,
                CategoryId = product.CategoryId,
                Brand = new BrandDs()
                {
                    Id = product.BrandId,
                    Label = product.Brand.Label
                },
                Discount = new DiscountDs()
                {
                    Id = product.DiscountId,
                    Value = product.Discount.Value,
                    ExpireDate = product.Discount.ExpireDate,
                    Type = EnumExtension<DiscountType>.GetDisplayName(product.Discount.Type)
                },
                Tags = productTags,
                Gallery = productGallery,
                ExtraFields = productExtraFields,
                Features = productFeatures
            };
            return productDs;
        }
        private void FillProductTags(Domain.Product.Product product, ref List<TagDs> tags)
        {
            foreach (var item in _productTagRepository.Get().Where(t => t.ProductId == product.Id))
            {
                tags.Add(new TagDs()
                {
                    Id = item.TagId,
                    Label = item.Tag.Label
                });
            }
        }
        private void FillProductFeatures(Domain.Product.Product product, ref List<ProductFeatureDs> features)
        {
            foreach (var item in _productFeatureRepository.Get().Where(t => t.ProductId == product.Id).ToList())
            {
                features.Add(new ProductFeatureDs()
                {
                    Feature = new FeatureDs()
                    {
                        Id = item.FeatureId,
                        Label = item.Feature.Label,
                        Type = EnumExtension<FeatureType>.GetDisplayName(item.Feature.Type),
                        Values = item.Feature.Values.Select(f => new FeatureValueDs()
                        {
                            Id = f.Id,
                            Label = f.Label,
                            ColorCode = f.ColorCode,
                            FeatureId = f.FeatureId
                        }).ToList()
                    },
                    Value = new BaseItem()
                    {
                        Id = item.FeatureValueId,
                        Label = item.FeatureValue.Label
                    },
                });
            }
        }
        private void FillProductGallery(Domain.Product.Product product, ref List<GalleryDs> gallery)
        {
            foreach (var item in _galleryRepository.Get().Where(t => t.ProductId == product.Id))
            {
                gallery.Add(new GalleryDs()
                {
                    Id = item.Id,
                    FileName = item.FileName
                });
            }
        }
        private void FillProductExtraFields(Domain.Product.Product product, ref List<ProductExtraFieldDs> extraFields)
        {
            foreach (var item in _productExtraFieldRepository.Get().Where(t => t.ProductId == product.Id))
            {
                extraFields.Add(new ProductExtraFieldDs()
                {
                    ExtraField = new ExtraFieldDs()
                    {
                        Id = item.ExtraFieldId,
                        Label = item.ExtraField.Label,
                        Type = EnumExtension<ExtraFieldType>.GetDisplayName(item.ExtraField.Type),
                        Group = new ExtraFieldGroupDs() { 
                            Id = item.ExtraField.ExtraFieldGroup.Id, 
                            Label = item.ExtraField.ExtraFieldGroup.Label 
                        },
                        CategoryIds = item.ExtraField.ExtraFieldCategories.Where(t => t.ExtraFieldId == item.ExtraFieldId).Select(item=>item.CategoryId).ToList()
                    },
                    Value = new BaseItem()
                    {
                        Id = item.ExtraFieldValueId,
                        Label = item.ExtraFieldValue.Value
                    },
                });
            }
        }
        private void AddProductTags(ICollection<TagDs> tags, int productId)
        {
            foreach (var tag in tags)
            {
                _productTagRepository.Add(new ProductTag()
                {
                    ProductId = productId,
                    TagId = tag.Id
                });
            }
            _productTagRepository.Save();
        }
        private void AddProductFeatures(ICollection<ProductFeatureDs> features, int productId)
        {
            foreach (var item in features)
            {
                _productFeatureRepository.Add(new ProductFeature()
                {
                    ProductId = productId,
                    FeatureId = item.Feature.Id,
                    FeatureValueId = item.Value.Id
                });
            }
            _productFeatureRepository.Save();
        }
        private void AddProductGallery(ICollection<GalleryDs> gallery, int productId)
        {
            foreach (var image in gallery)
            {
                _galleryRepository.Add(new Gallery()
                {
                    ProductId = productId,
                    FileName = image.FileName
                });
            }
            _galleryRepository.Save();
        }
        private void AddProductExtraFields(ICollection<ProductExtraFieldDs> extraFields, int productId)
        {
            foreach (var item in extraFields)
            {
                _productExtraFieldRepository.Add(new ProductExtraField()
                {
                    ProductId = productId,
                    ExtraFieldId = item.ExtraField.Id,
                    ExtraFieldValueId = item.Value.Id
                });
            }
            _productExtraFieldRepository.Save();
        }
        private void DeleteProductTags(ProductDs product)
        {
            var productTags = _productTagRepository.Get().Where(item => item.ProductId == product.Id);
            foreach (var productTag in productTags)
            {
                _productTagRepository.Delete(productTag);
            }
        }
        private void DeleteProductFeatures(ProductDs product)
        {
            var productFeatures = _productFeatureRepository.Get().Where(item => item.ProductId == product.Id);
            foreach (var productFeature in productFeatures)
            {
                _productExtraFieldRepository.Delete(productFeature);
            }
        }
        private void DeleteProductGallery(ProductDs product)
        {
            var productGallery = _galleryRepository.Get().Where(item => item.ProductId == product.Id);
            foreach (var item in productGallery)
            {
                _galleryRepository.Delete(item);
            }
        }
        private void DeleteProductExtraFields(ProductDs product)
        {
            var productExtraFields = _productExtraFieldRepository.Get().Where(item => item.ProductId == product.Id);
            foreach (var item in productExtraFields)
            {
                _productExtraFieldRepository.Delete(item);
            }
        }
        private void UpdateProductTags(ProductDs product)
        {
            DeleteProductTags(product);
            AddProductTags(product.Tags, product.Id);
        }
        private void UpdateProductFeatures(ProductDs product)
        {
            DeleteProductFeatures(product);
            AddProductFeatures(product.Features, product.Id);
        }
        private void UpdateProductGallery(ProductDs product)
        {
            DeleteProductGallery(product);
            AddProductGallery(product.Gallery, product.Id);
        }
        private void UpdateProductExtraFields(ProductDs product)
        {
            DeleteProductExtraFields(product);
            AddProductExtraFields(product.ExtraFields, product.Id);
        }
        public bool Exist(object id)
        {
            return _productRepository.Get().Any(p => p.Id == (int)id);
        }
        public bool IsDublicate(ProductDs product)
        {
            return _productRepository.Get().Any(p => p.Name == product.Name);
        }
    }
}
