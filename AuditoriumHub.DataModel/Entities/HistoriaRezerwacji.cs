using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
   public class HistoriaRezerwacji : IEntity
    {
        public int Id { get; set; }
        public int RezerwacjaId { get; set; }
        public int UzytkownikId { get; set; }
        public DateTime DataZmiany { get; set; }
        public string RodzajOperacji { get; set; } = string.Empty;
        public string OpisZmiany { get; set; } = string.Empty;
    }
}
