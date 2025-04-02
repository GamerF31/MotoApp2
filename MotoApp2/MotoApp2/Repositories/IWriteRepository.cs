using MotoApp2.Entities;
using System.Security.Principal;
namespace MotoApp2.Entities.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void Add(T item);
        void Remove(T item);
        void Save();
    }
}
