using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.DataModel.Entities.Users;
using AuditoriumHub.DataAccess.Memory.Context;
using AuditoriumHub.DataAccess.Memory.Repositories;
namespace AuditoriumHub.DataAccess.Memory.UnitOfWork
{
    public class UnitOfWorkMemory : IUnitOfWork
    {
        public IRepository<Sala> Sale { get; }
        public IRepository<Budynek> Budynki { get; }
        public IRepository<Rezerwacja> Rezerwacje { get; }
        public IRepository<HistoriaRezerwacji> HistoriaRezerwacji { get; }
        public IRepository<Uzytkownik> Uzytkownicy { get; }
        public IRepository<Wyposazenie> Wyposazenia { get; }
        public IRepository<WyposazenieSali> WyposazenieSali { get; }


        public UnitOfWorkMemory(MemoryDbContext context)
        {

            ArgumentNullException.ThrowIfNull(context);
            Sale = new RepositoryMemory<Sala>(context);
            Budynki = new RepositoryMemory<Budynek>(context);
            Rezerwacje = new RepositoryMemory<Rezerwacja>(context);
            HistoriaRezerwacji = new RepositoryMemory<HistoriaRezerwacji>(context);
            Uzytkownicy = new RepositoryMemory<Uzytkownik>(context);
            Wyposazenia = new RepositoryMemory<Wyposazenie>(context);
            WyposazenieSali = new RepositoryMemory<WyposazenieSali>(context);

        }

        public void SaveChanges()
        {
            //TODO: W przypadku pamięci podręcznej nie ma potrzeby zapisywania zmian, ponieważ wszystkie operacje są wykonywane bezpośrednio na obiektach w pamięci.
        }



    }
}
