using AuditoriumHub.DataModel.Enums;
using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
   public class Rezerwacja : IEntity
   {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public int UzytkownikId { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public string Cel { get; set; } = string.Empty;
        public StatusRezerwacji Status { get; set; } = StatusRezerwacji.Aktywna;

        public void ZmienTermin(DateTime Od, DateTime Do)
        {
            if (Od >= Do) throw new ArgumentException("Data rozpoczęcia musi być wcześniejsza niż data zakończenia.");
            DataOd = Od;
            DataDo = Do;
        }

        public void Anuluj() => Status = StatusRezerwacji.Anulowana;

        public bool CzyKoliduje(Rezerwacja inna) => 
            SalaId == inna.SalaId &&
            Status == StatusRezerwacji.Aktywna &&
            inna.Status == StatusRezerwacji.Aktywna &&
            DataOd < inna.DataDo &&
            inna.DataOd < DataDo;


    }
}
