using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using System.Linq;
using ShopApi.Interfaces.Product;

namespace ShopApi.Services.Product
{
    public class ExtraFieldGroupService : IExtraFieldGroup
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<ExtraFieldGroup> _extraFieldGroupRepository;

        public ExtraFieldGroupService(DatabaseContext context)
        {
            _extraFieldGroupRepository = new GenericRepository<ExtraFieldGroup>(context);
        }

        public CrudOperationDs GetAll()
        {
            _crudDs.Result = _extraFieldGroupRepository.Get().Select(extraFieldGroup => new ExtraFieldGroupDs()
            {
                Id = extraFieldGroup.Id,
                Label = extraFieldGroup.Label
            });
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var extraFieldGroup = _extraFieldGroupRepository.GetById(id);
                var result = new ExtraFieldGroupDs()
                {
                    Id = extraFieldGroup.Id,
                    Label = extraFieldGroup.Label
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(ExtraFieldGroupDs extraFieldGroup)
        {
            if (IsDublicate(extraFieldGroup))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result = _extraFieldGroupRepository.Add(new ExtraFieldGroup()
                {
                    Id = extraFieldGroup.Id,
                    Label = extraFieldGroup.Label
                });
                try
                {
                    _extraFieldGroupRepository.Save();
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
            var extraFieldGroup = _extraFieldGroupRepository.GetById(id);
            _extraFieldGroupRepository.Delete(extraFieldGroup);
            try
            {
                _extraFieldGroupRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = extraFieldGroup;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(ExtraFieldGroupDs extraFieldGroup)
        {
            var result = _extraFieldGroupRepository.Update(new ExtraFieldGroup()
            {
                Id = extraFieldGroup.Id,
                Label = extraFieldGroup.Label
            });
            try
            {
                _extraFieldGroupRepository.Save();
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
            return _extraFieldGroupRepository.Get().Any(e => e.Id == (int)id);
        }

        public bool IsDublicate(ExtraFieldGroupDs extraFieldGroup)
        {
            return _extraFieldGroupRepository.Get().Any(e => e.Label == extraFieldGroup.Label);
        }
    }
}
