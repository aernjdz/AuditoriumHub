using AuditoriumHub.DataAccess.Memory.Context;
using AuditoriumHub.DataModel.Interfaces;
using AuditoriumHub.ServiceAbstractions.Repositories;
namespace AuditoriumHub.DataAccess.Memory.Repositories
{
    public class RepositoryMemory<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _items;

        public RepositoryMemory(MemoryDbContext context)
        {
            _items = context.Set<T>();
        }
        public T? GetById(int id) {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T entity)
        {
            if (_items.Any(x => x.Id == entity.Id)) throw new InvalidOperationException($"Entity with Id {entity.Id} already exists.");
            
            _items.Add(entity);

        }

        public void Update(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing == null) throw new InvalidOperationException($"Entity with Id {entity.Id} does not exist.");
            
            int idx =_items.IndexOf(existing);

            _items[idx] = entity;

        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) throw new InvalidOperationException($"Entity with Id {id} does not exist.");
            _items.Remove(existing);
        }
    }
}
