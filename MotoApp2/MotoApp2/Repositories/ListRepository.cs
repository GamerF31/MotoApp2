namespace MotoApp2.Entities.Repositories
{
    using MotoApp2.Entities;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Principal;

    public class ListRepository<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        private readonly List<T> _items = new();
        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }
        public T CreateNewItem()
        {
            return new T();
        }
        public void Add(T item)
        {
            item.Id = _items.Count + 1;
            _items.Add(item);
        }

        public void Save()
        {
            foreach (var item in _items)
            {
                Console.WriteLine(item);
            }
        }

        public T GetById(int id)
        {

            return _items.Single(item => item.Id == id);
        }


        public void Remove(T item)
        {
            throw new NotImplementedException();
        }
    }
}
