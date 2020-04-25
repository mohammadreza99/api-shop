using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using ShopApi.Interfaces.Product;
using System.Linq;
using ShopApi.DataLayer.Data;
using System.Collections.Generic;
using ShopApi.Interfaces;

namespace ShopApi.Services.Product
{
    public class CategoryService : ICategory
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<Category> _categoryRepository;
        private readonly GenericRepository<ExtraFieldCategory> _extraFieldCategoryRepository;

        public CategoryService(DatabaseContext context)
        {
            _categoryRepository = new GenericRepository<Category>(context);
            _extraFieldCategoryRepository = new GenericRepository<ExtraFieldCategory>(context);
        }

        public CrudOperationDs GetAll()
        {
            var extraFieldIds = _extraFieldCategoryRepository.Get().Select(item => item.ExtraFieldId).ToList();
            _crudDs.Result = _categoryRepository.Get().Select(c => new CategoryDs()
            {
                Id = c.Id,
                Label = c.Label,
                ParentId = c.Parent.Id,
                ExtraFieldIds = extraFieldIds
            }).ToList();
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var category = _categoryRepository.GetById(id);
                var extraFieldIds = _extraFieldCategoryRepository.Get().Where(e => e.CategoryId == (int)id).Select(e=>e.ExtraFieldId).ToList();
                var result = new CategoryDs()
                {
                    Id = category.Id,
                    Label = category.Label,
                    ParentId = category.ParentId,
                    ExtraFieldIds = extraFieldIds
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(CategoryDs category)
        {
            if (IsDublicate(category))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                var result = _categoryRepository.Add(new Category()
                {
                    Label = category.Label,
                    ParentId = category.ParentId
                });
                _categoryRepository.Save();
                if (category.ExtraFieldIds != null)
                    foreach (var id in category.ExtraFieldIds)
                    {
                        _extraFieldCategoryRepository.Add(new ExtraFieldCategory()
                        {
                            CategoryId = result.Id,
                            ExtraFieldId = id
                        });
                    }
                try
                {
                    _extraFieldCategoryRepository.Save();
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
            var category = _categoryRepository.GetById(id);
            _categoryRepository.Delete(category);
            try
            {
                _categoryRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = category;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(CategoryDs category)
        {
            if (!Exist(category.Id))
                _crudDs.SetError("دسته بندی یافت نشد.");
            else
            {
                var result = _categoryRepository.Update(new Category()
                {
                    Id = category.Id,
                    Label = category.Label,
                    ParentId = category.ParentId
                });
                var extraFieldCategories = _extraFieldCategoryRepository.Get().Where(e => e.CategoryId == category.Id).ToList();
                foreach (var item in extraFieldCategories)
                {
                    _extraFieldCategoryRepository.Delete(item);
                }
                foreach (var item in category.ExtraFieldIds)
                {
                    _extraFieldCategoryRepository.Add(new ExtraFieldCategory()
                    {
                        ExtraFieldId = item,
                        CategoryId = category.Id
                    });
                }
                _extraFieldCategoryRepository.Save();
                try
                {
                    _categoryRepository.Save();
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

        public bool Exist(object id)
        {
            return _categoryRepository.Get().Any(c => c.Id == (int)id);
        }

        public bool IsDublicate(CategoryDs category)
        {
            return _categoryRepository.Get().Any(c => c.Label == category.Label && c.ParentId == category.ParentId);
        }
    }
}

