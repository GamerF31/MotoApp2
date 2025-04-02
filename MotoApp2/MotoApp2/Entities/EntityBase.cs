using System.Security.Principal;

namespace MotoApp2.Entities
{
    public abstract class EntityBase : IEntity
    {
        public int Id { get; set; }
    }
}
