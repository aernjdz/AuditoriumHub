using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
    public class Sala : IEntity
    {
        public int Id { get;  set;}
        public string Numer { get; set; } = string.Empty;
        public int Pojemnosc { get; set; }

        public bool CzyAktywna { get; set; } = true;
        public int BudynekId { get; set; }
        public Budynek? Budynek { get; set; }
        public ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
        public ICollection<WyposazenieSali> WyposazenieSal { get; set; } = new List<WyposazenieSali>();
        public void Dezaktywuj() => CzyAktywna = false;



    }
}
