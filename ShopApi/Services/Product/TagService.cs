using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using ShopApi.Interfaces.Product;
using System.Linq;

namespace ShopApi.Services.Product
{
    public class TagService : ITag
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<Tag> _tagRepository;

        public TagService(DatabaseContext context)
        {
            _tagRepository = new GenericRepository<Tag>(context);
        }

        public CrudOperationDs GetAll()
        {
            _crudDs.Result = _tagRepository.Get().Select(tag => new TagDs()
            {
                Id = tag.Id,
                Label = tag.Label,
            }).ToList();
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var tag = _tagRepository.GetById(id);
                var result = new TagDs()
                {
                    Id = tag.Id,
                    Label = tag.Label
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("تگ یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(TagDs tag)
        {
            if (IsDublicate(tag))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result= _tagRepository.Add(new Tag()
                {
                    Id = tag.Id,
                    Label = tag.Label
                });
                try
                {
                    _tagRepository.Save();
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
            var tag = _tagRepository.GetById(id);
            _tagRepository.Delete(tag);
            try
            {
                _tagRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = tag;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(TagDs tag)
        {
            var result = _tagRepository.Update(new Tag()
            {
                Id = tag.Id,
                Label = tag.Label,
            });
            try
            {
                _tagRepository.Save();
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
            return _tagRepository.Get().Any(c => c.Id == (int)id);
        }

        public bool IsDublicate(TagDs tag)
        {
            return _tagRepository.Get().Any(t => t.Label == tag.Label);
        }
    }
}
