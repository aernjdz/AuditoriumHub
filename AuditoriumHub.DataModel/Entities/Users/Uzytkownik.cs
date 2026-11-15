using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities.Users
{
    public abstract class Uzytkownik : IEntity
    {
        public int Id { get; set; }
        public string Imie { get; set; } = string.Empty;

        public string Nazwisko { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool CzyAktywny { get; set; } = true;
        public void Dezaktywuj() => CzyAktywny = false;



    }
}
