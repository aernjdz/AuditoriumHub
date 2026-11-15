using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.DataModel.Entities.Users;

namespace AuditoriumHub.ServiceAbstractions.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Sala> Sale { get; }
        IRepository<Budynek> Budynki { get; }
        IRepository<Rezerwacja> Rezerwacje { get; }
        IRepository<HistoriaRezerwacji> HistoriaRezerwacji { get; }
        IRepository<Uzytkownik> Uzytkownicy { get; }
        IRepository<Wyposazenie> Wyposazenia { get; }
        IRepository<WyposazenieSali> WyposazenieSali { get; }
        
        void SaveChanges();
    }
}
