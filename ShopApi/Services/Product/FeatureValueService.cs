using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.DataLayer.Data;
using ShopApi.Interfaces.Product;

namespace ShopApi.Services.Product
{
    public class FeatureValueService : IFeatureValue
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<FeatureValue> _featureValueRepository;

        public FeatureValueService(DatabaseContext context)
        {
            _featureValueRepository = new GenericRepository<FeatureValue>(context);
        }

        public CrudOperationDs GetAll()
        {
            _crudDs.Result = _featureValueRepository.Get().Select(featureValue => new FeatureValueDs()
            {
                Id = featureValue.Id,
                Label = featureValue.Label,
            }).ToList(); ;
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var featureValue = _featureValueRepository.GetById(id);
                var result = new FeatureValueDs()
                {
                    Id = featureValue.Id,
                    Label = featureValue.Label,
                    ColorCode = featureValue.ColorCode
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(FeatureValueDs featureValue)
        {
            if (IsDublicate(featureValue))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result = _featureValueRepository.Add(new FeatureValue()
                {
                    Id = featureValue.Id,
                    ColorCode = featureValue.ColorCode,
                    Label = featureValue.Label,
                    FeatureId = featureValue.FeatureId
                });
                try
                {
                    _featureValueRepository.Save();
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
            var feature = _featureValueRepository.GetById(id);
            _featureValueRepository.Delete(feature);
            try
            {
                _featureValueRepository.Save();
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

        public CrudOperationDs Update(FeatureValueDs featureValue)
        {
            var result = _featureValueRepository.Update(new FeatureValue()
            {
                Id = featureValue.Id,
                Label = featureValue.Label,
                ColorCode = featureValue.ColorCode
            });
            try
            {
                _featureValueRepository.Save();
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

        public bool Exist(object id)
        {
            return _featureValueRepository.Get().Any(f => f.Id == (int)id);
        }

        public bool IsDublicate(FeatureValueDs featureValue)
        {
            return _featureValueRepository.Get().Any(f => f.Label == featureValue.Label);
        }
    }
}
