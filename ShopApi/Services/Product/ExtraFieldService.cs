using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Repositories;
using ShopApi.Domain.Product;
using System.Collections.Generic;
using System.Linq;
using ShopApi.DataLayer.Data;
using ShopApi.Interfaces.Product;
using ShopApi.DataLayer.Method;
using System;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Services.Product
{
    public class ExtraFieldService : IExtraField
    {
        private readonly CrudOperationDs _crudDs = new CrudOperationDs();
        private readonly GenericRepository<ExtraField> _extraFieldRepository;
        private readonly GenericRepository<ExtraFieldValue> _extraFieldValueRepository;
        private readonly GenericRepository<ExtraFieldGroup> _extraFieldGroupRepository;
        private readonly ExtraFieldCategoryRepository _extraFieldCategoryRepository;

        public ExtraFieldService(DatabaseContext context)
        {
            _extraFieldRepository = new GenericRepository<ExtraField>(context);
            _extraFieldValueRepository = new GenericRepository<ExtraFieldValue>(context);
            _extraFieldGroupRepository = new GenericRepository<ExtraFieldGroup>(context);
            _extraFieldCategoryRepository = new ExtraFieldCategoryRepository(context);
        }

        public CrudOperationDs GetAll()
        {
            var result = new List<ExtraFieldDs>();
            foreach (var extraField in _extraFieldRepository.Get().Include(item => item.ExtraFieldGroup).ToList())
            {
                result.Add(new ExtraFieldDs()
                {
                    Id = extraField.Id,
                    Label = extraField.Label,
                    Type = EnumExtension<ExtraFieldType>.GetDisplayName(extraField.Type),
                    Group = new ExtraFieldGroupDs()
                    {
                        Id = _extraFieldGroupRepository.GetById(extraField.ParentId).Id,
                        Label = _extraFieldGroupRepository.GetById(extraField.ParentId).Label
                    },
                    CategoryIds = _extraFieldCategoryRepository.Get().Where(t => t.ExtraFieldId == extraField.Id).Select(item => item.CategoryId).ToList(),
                    ListItems = _extraFieldValueRepository.Get().Where(item => item.ExtraFieldId == extraField.Id).Select(item => item.Value).ToList()
                });
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public CrudOperationDs GetById(object id)
        {
            if (Exist(id))
            {
                var extraField = _extraFieldRepository.GetById(id);
                var categoryIds =
                    _extraFieldCategoryRepository.Get().Where(c => c.ExtraFieldId == extraField.Id).Select(item => item.CategoryId).ToList();
                var listItems = _extraFieldValueRepository.Get().Where(c => c.ExtraFieldId == extraField.Id).Select(item => item.Value).ToList();
                var result = new ExtraFieldDs()
                {
                    Id = extraField.Id,
                    Label = extraField.Label,
                    Type = EnumExtension<ExtraFieldType>.GetDisplayName(extraField.Type),
                    Group = new ExtraFieldGroupDs
                    {
                        Id = _extraFieldGroupRepository.GetById(extraField.ParentId).Id,
                        Label = _extraFieldGroupRepository.GetById(extraField.ParentId).Label
                    },
                    CategoryIds = categoryIds,
                    ListItems = listItems
                };
                _crudDs.Result = result;
            }
            else
                _crudDs.SetError("دسته بندی یافت نشد.");
            return _crudDs;
        }

        public CrudOperationDs Add(ExtraFieldDs extraField)
        {
            if (IsDublicate(extraField))
                _crudDs.SetError("دسته بندی با این نام قبلا ثبت شده است");
            else
            {
                AddExtraField(extraField);
                _crudDs.Result = extraField;
            }
            return _crudDs;
        }

        public CrudOperationDs Delete(object id)
        {
            var extraField = _extraFieldRepository.GetById(id);
            _extraFieldRepository.Delete(extraField);
            try
            {
                _extraFieldRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
            finally
            {
                _crudDs.Result = extraField;
            }
            return _crudDs;
        }

        public CrudOperationDs Update(ExtraFieldDs extraField)
        {
            UpdateExtraField(extraField);
            _crudDs.Result = extraField;
            return _crudDs;
        }

        public CrudOperationDs GetExtraFieldByGroupId(int groupId)
        {
            var result = new List<ExtraFieldDs>();
            foreach (var extraField in _extraFieldRepository.Get().Where(e => e.ParentId == groupId).ToList())
            {
                result.Add(new ExtraFieldDs()
                {
                    Id = extraField.Id,
                    Label = extraField.Label,
                    Type = EnumExtension<ExtraFieldType>.GetDisplayName(extraField.Type),
                    Group = new ExtraFieldGroupDs() {
                        Id = _extraFieldGroupRepository.GetById(extraField.ParentId).Id,
                        Label = _extraFieldGroupRepository.GetById(extraField.ParentId).Label
                    },
                    CategoryIds = _extraFieldCategoryRepository.Get().Where(t => t.ExtraFieldId == extraField.Id).Select(item => item.CategoryId).ToList(),
                    ListItems = _extraFieldValueRepository.Get().Where(item => item.ExtraFieldId == extraField.Id).Select(item => item.Value).ToList()
                });
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public CrudOperationDs GetExtraFieldTypes()
        {
            var result = new List<string>();
            foreach (var item in Enum.GetValues(typeof(ExtraFieldType)))
            {
                result.Add(EnumExtension<ExtraFieldType>.GetDisplayName((ExtraFieldType)item));
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public CrudOperationDs GetExtraFieldsByCategoryId(int id)
        {
            var result = new List<ProductExtraFieldDs>();
            var extrafieldIds = _extraFieldCategoryRepository.Get().Where(item => item.CategoryId == id).Select(item => item.ExtraFieldId).ToList();
            foreach (var extrafieldId in extrafieldIds)
            {
                result.Add(new ProductExtraFieldDs()
                {
                    ExtraField = (ExtraFieldDs)GetById(extrafieldId).Result
                });
            }
            _crudDs.Result = result;
            return _crudDs;
        }

        public bool Exist(object id)
        {
            return _extraFieldRepository.Get().Any(e => e.Id == (int)id);
        }

        public bool IsDublicate(ExtraFieldDs extraField)
        {
            return _extraFieldRepository.Get().Any(e => e.Label == extraField.Label);
        }

        private void AddExtraField(ExtraFieldDs extraField)
        {
            var result = _extraFieldRepository.Add(new ExtraField()
            {
                Label = extraField.Label,
                ParentId = extraField.Group.Id,
                Type = EnumExtension<ExtraFieldType>.GetValueFromName(extraField.Type)
            });
            try
            {
                _extraFieldRepository.Save();
                extraField.Id = result.Id;
                AddExtraFieldCategories(extraField);
                AddExtraFieldListItems(extraField);
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
        }

        private void AddExtraFieldCategories(ExtraFieldDs extraField)
        {
            foreach (var id in extraField.CategoryIds)
            {
                _extraFieldCategoryRepository.Add(new ExtraFieldCategory()
                {
                    CategoryId = id,
                    ExtraFieldId = extraField.Id
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
        }

        private void AddExtraFieldListItems(ExtraFieldDs extraField)
        {
            foreach (var item in extraField.ListItems)
            {
                _extraFieldValueRepository.Add(new ExtraFieldValue()
                {
                    ExtraFieldId = extraField.Id,
                    Value = item
                });
            }
            try
            {
                _extraFieldValueRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
        }

        private void UpdateExtraField(ExtraFieldDs extraField)
        {
            _extraFieldRepository.Update(new ExtraField()
            {
                Id = extraField.Id,
                Label = extraField.Label,
                ParentId = extraField.Group.Id,
                Type = EnumExtension<ExtraFieldType>.GetValueFromName(extraField.Type)
            });
            UpdateExtraFieldCategories(extraField);
            UpdateExtraFieldListItems(extraField);
            try
            {
                _extraFieldRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
        }

        private void UpdateExtraFieldCategories(ExtraFieldDs extraField)
        {
            var extraFieldCategories = _extraFieldCategoryRepository.Get().Where(item => item.ExtraFieldId == extraField.Id).ToList();
            if (extraFieldCategories != null)
            {
                foreach (var item in extraFieldCategories)
                {
                    _extraFieldCategoryRepository.Delete(item);
                }
                foreach (var id in extraField.CategoryIds)
                {
                    _extraFieldCategoryRepository.Add(new ExtraFieldCategory()
                    {
                        CategoryId = id,
                        ExtraFieldId = extraField.Id
                    });
                }
            }
            try
            {
                _extraFieldCategoryRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
        }

        private void UpdateExtraFieldListItems(ExtraFieldDs extraField)
        {
            var extraFieldValues = _extraFieldValueRepository.Get().Where(item => item.ExtraFieldId == extraField.Id).ToList();
            if (extraFieldValues != null)
            {
                foreach (var item in extraFieldValues)
                {
                    _extraFieldValueRepository.Delete(item);
                }
                foreach (var item in extraField.ListItems)
                {
                    _extraFieldValueRepository.Add(new ExtraFieldValue()
                    {
                        Value = item,
                        ExtraFieldId = extraField.Id
                    });
                }
            }
            try
            {
                _extraFieldValueRepository.Save();
            }
            catch
            {
                _crudDs.SetError("عملیات با خطا مواجه شد.");
            }
        }
    }
}
