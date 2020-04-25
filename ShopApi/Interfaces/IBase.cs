using ShopApi.DataLayer.DataStructure;

namespace ShopApi.Interfaces
{
    public interface IBase<T> where T : class
    {
        public CrudOperationDs GetAll();
        public CrudOperationDs GetById(object id);
        public CrudOperationDs Add(T entity);
        public CrudOperationDs Delete(object id);
        public CrudOperationDs Update(T entity);
        public bool Exist(object id);
        public bool IsDublicate(T entity);
    }
}
