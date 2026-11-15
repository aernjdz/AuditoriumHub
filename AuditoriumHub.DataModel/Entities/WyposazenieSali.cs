using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
    public class WyposazenieSali : IEntity
    {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public int  WyposazenieId { get; set; }
        public int Ilosc { get; set; }

        public  void ZmienIlosc(int ilosc)
        {
            if (ilosc < 0) throw new ArgumentOutOfRangeException(nameof(ilosc));
            Ilosc = ilosc;
        }

    }
}
