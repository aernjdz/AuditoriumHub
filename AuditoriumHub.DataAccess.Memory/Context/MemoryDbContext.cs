using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.DataModel.Entities.Users;
using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataAccess.Memory.Context
{
    public class MemoryDbContext
    {
        public List<Sala> Sale { get; set; } = new();
        public List<Budynek> Budynki { get; set; } = new();
        public List<Rezerwacja> Rezerwacje { get; set; } = new ();
        public List<HistoriaRezerwacji> HistoriaRezerwacji { get; set; } = new();
        public List<Uzytkownik> Uzytkownicy { get; set; } = new ();
        public List<Wyposazenie> Wyposazenia { get; set; } = new();
        public List<WyposazenieSali> WyposazenieSali { get; set; } = new();
    
    
        public List<T> Set<T>() where T: class, IEntity
        {
            if (typeof(T) == typeof(Sala))
                return (List<T>)(object)Sale;
            else if (typeof(T) == typeof(Budynek))
                return (List<T>)(object)Budynki;
            else if (typeof(T) == typeof(Rezerwacja))
                return (List<T>)(object)Rezerwacje;
            else if (typeof(T) == typeof(HistoriaRezerwacji))
                return (List<T>)(object)HistoriaRezerwacji;
            else if (typeof(T) == typeof(Uzytkownik))
                return (List<T>)(object)Uzytkownicy;
            else if (typeof(T) == typeof(Wyposazenie))
                return (List<T>)(object)Wyposazenia;
            else if (typeof(T) == typeof(WyposazenieSali))
                return (List<T>)(object)WyposazenieSali;
            else
                throw new InvalidOperationException($"Nieobsługiwany typ encji: {typeof(T).Name}");
        }
    }


    
}
