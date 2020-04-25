using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.Interfaces.Product;
using System.Collections.Generic;

namespace ShopApi.Services.Product
{
    public class BrandService : IBrand
    {
        private readonly CrudOperationDs _crudDs;
        private readonly GenericRepository<Brand> _brandRepository;

        public BrandService(DatabaseContext context)
        {
            _crudDs = new CrudOperationDs();
            _brandRepository = new GenericRepository<Brand>(context);
        }

        public CrudOperationDs GetAll()
        {
            _crudDs.Result = _brandRepository.Get().Select(brand => new BrandDs()
            {
                Id = brand.Id,
                Label = brand.Label
            }).ToList();
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var brand = _brandRepository.GetById(id);
                var result = new BrandDs()
                {
                    Id = brand.Id,
                    Label = brand.Label
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("برندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(BrandDs brand)
        {
            if (IsDublicate(brand))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result = _brandRepository.Add(new Brand()
                {
                    Id = brand.Id,
                    Label = brand.Label,
                });
                try
                {
                    _brandRepository.Save();
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
            var brand = _brandRepository.GetById(id);
            _brandRepository.Delete(brand);
            try
            {
                _brandRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = brand;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(BrandDs brand)
        {
            var result = _brandRepository.Update(new Brand()
            {
                Id = brand.Id,
                Label = brand.Label,
            });
            try
            {
                _brandRepository.Save();
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
            return _brandRepository.Get().Any(b => b.Id == (int)id);
        }

        public bool IsDublicate(BrandDs brand)
        {
            return _brandRepository.Get().Any(b => b.Label == brand.Label);
        }
    }
}
