using System.Threading.Tasks;

namespace KlicKitApi.Data
{
    public interface IUnitOfWork
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
    }
}