using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.ServiceAbstractions.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        T? GetById(int id);
        List<T> GetAll();

        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
       

    }
}
