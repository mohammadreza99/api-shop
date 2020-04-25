using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.DataLayer.Data;
using ShopApi.Interfaces.Product;
using System.Collections.Generic;
using ShopApi.DataLayer.Method;
using System;

namespace ShopApi.Services.Product
{
    public class FeatureService : IFeature
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<Feature> _featureRepository;
        private readonly GenericRepository<FeatureValue> _featureValueRepository;

        public FeatureService(DatabaseContext context)
        {
            _featureRepository = new GenericRepository<Feature>(context);
            _featureValueRepository = new GenericRepository<FeatureValue>(context);
        }

        public CrudOperationDs GetAll()
        {
            var result = new List<FeatureDs>();
            foreach (var feature in _featureRepository.Get().ToList())
            {
                var featureValues = new List<FeatureValueDs>();
                foreach (var item in _featureValueRepository.Get().Where(t => t.FeatureId == feature.Id).ToList())
                {
                    featureValues.Add(new FeatureValueDs()
                    {
                        Id = item.Id,
                        Label = item.Label,
                        ColorCode = item.ColorCode
                    });
                }
                result.Add(new FeatureDs()
                {
                    Id = feature.Id,
                    Label = feature.Label,
                    Type = EnumExtension<FeatureType>.GetDisplayName(feature.Type),
                    Values = featureValues
                });
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var feature = _featureRepository.GetById(id);
                var featureValues = _featureValueRepository.Get().Where(t => t.FeatureId == feature.Id).Select(item => new FeatureValueDs()
                {
                    Id = item.Id,
                    Label = item.Label,
                    ColorCode = item.ColorCode
                }).ToList();
                var result = new FeatureDs()
                {
                    Id = feature.Id,
                    Label = feature.Label,
                    Type = EnumExtension<FeatureType>.GetDisplayName(feature.Type),
                    Values = featureValues
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(FeatureDs feature)
        {
            if (IsDublicate(feature))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result = _featureRepository.Add(new Feature()
                {
                    Id = feature.Id,
                    Label = feature.Label,
                    Type = EnumExtension<FeatureType>.GetValueFromName(feature.Type)
                });
                try
                {
                    _featureRepository.Save();
                }
                catch
                {
                    _crudDs.SetError("عملیات با خطا مواجه شد.");
                }
                finally
                {
                    _crudDs.Result = result;
                }
            }
            return _crudDs;
        }

        public CrudOperationDs Delete(object id)
        {
            var feature = _featureRepository.GetById(id);
            _featureRepository.Delete(feature);
            try
            {
                _featureRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = feature;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(FeatureDs feature)
        {
            var result = _featureRepository.Update(new Feature()
            {
                Id = feature.Id,
                Label = feature.Label,
                Type = EnumExtension<FeatureType>.GetValueFromName(feature.Type)
            });
            try
            {
                _featureRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = result;
            }
            return _crudDs;
        }

        public CrudOperationDs GetFeatureTypes()
        {
            var result = new List<string>();
            foreach (var item in Enum.GetValues(typeof(FeatureType)))
            {
                result.Add(EnumExtension<FeatureType>.GetDisplayName((FeatureType)item));
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public bool Exist(object id)
        {
            return _featureRepository.Get().Any(f => f.Id == (int)id);
        }

        public bool IsDublicate(FeatureDs feature)
        {
            return _featureRepository.Get().Any(f => f.Label == feature.Label);
        }
    }
}
