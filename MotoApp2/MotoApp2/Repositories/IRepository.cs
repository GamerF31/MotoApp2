using System.Security.Principal;

namespace MotoApp2.Entities.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class, IEntity
    {
    }
}
