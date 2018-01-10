using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWS.Shared.DataAccess.Abstract
{
    public abstract class BaseStorageOfT<T>
    {
        public abstract Task<IEnumerable<T>> GetEntitiesAsync();
        public abstract Task SaveEntityAsync(T entity);
        //TODO:
        //Add update and get by id methods
    }
}
