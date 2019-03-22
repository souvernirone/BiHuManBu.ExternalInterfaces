using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Repository.DataContextStorage
{
    public interface IDataContextStorageContainer
    {
        EntityContext GetDataContext();
        void Store(EntityContext libraryDataContext);
    }
}
