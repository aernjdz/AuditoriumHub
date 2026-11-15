using AuditoriumHub.DataModel.Entities.Users;
using AuditoriumHub.ServiceAbstractions.Services;
using AuditoriumHub.ServiceAbstractions.Repositories;

namespace AuditoriumHub.Services.Memory.Services
{
    public class UzytkownikService : IUzytkownikService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UzytkownikService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DodajUzytkownika(Uzytkownik uzytkownik)
        {
            ArgumentNullException.ThrowIfNull(uzytkownik);

            if (string.IsNullOrWhiteSpace(uzytkownik.Imie) ||
                string.IsNullOrWhiteSpace(uzytkownik.Nazwisko) ||
                string.IsNullOrWhiteSpace(uzytkownik.Email))
                throw new ArgumentException(
                    "Imię, nazwisko i email są wymagane.");

            if (_unitOfWork.Uzytkownicy.GetAll()
                .Any(u => string.Equals(u.Email, uzytkownik.Email,
                    StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException(
                    "Użytkownik o podanym adresie email już istnieje.");

            _unitOfWork.Uzytkownicy.Add(uzytkownik);
            _unitOfWork.SaveChanges();
        }

        public void EdytujUzytkownika(Uzytkownik uzytkownik)
        {
            ArgumentNullException.ThrowIfNull(uzytkownik);

            var existing = _unitOfWork.Uzytkownicy.GetById(uzytkownik.Id);

            if (existing == null)
                throw new InvalidOperationException(
                    "Użytkownik nie istnieje.");

            if (string.IsNullOrWhiteSpace(uzytkownik.Imie) ||
                string.IsNullOrWhiteSpace(uzytkownik.Nazwisko) ||
                string.IsNullOrWhiteSpace(uzytkownik.Email))
                throw new ArgumentException(
                    "Imię, nazwisko i email są wymagane.");

            bool emailZajety = _unitOfWork.Uzytkownicy.GetAll()
                .Any(u => u.Id != uzytkownik.Id &&
                    string.Equals(u.Email, uzytkownik.Email,
                        StringComparison.OrdinalIgnoreCase));

            if (emailZajety)
                throw new InvalidOperationException(
                    "Adres email jest już używany.");

            existing.Imie = uzytkownik.Imie;
            existing.Nazwisko = uzytkownik.Nazwisko;
            existing.Email = uzytkownik.Email;

            _unitOfWork.SaveChanges();
        }

        public void UsunUzytkownika(int id)
        {
            bool maRezerwacje = _unitOfWork.Rezerwacje.GetAll()
                .Any(r => r.UzytkownikId == id);

            if (maRezerwacje)
                throw new InvalidOperationException(
                    "Nie można usunąć użytkownika posiadającego historię rezerwacji.");

            _unitOfWork.Uzytkownicy.Delete(id);
            _unitOfWork.SaveChanges();
        }

        public void DezaktywujUzytkownika(int id)
        {
            var existing = _unitOfWork.Uzytkownicy.GetById(id);

            if (existing == null)
                throw new InvalidOperationException(
                    "Użytkownik nie istnieje.");

            existing.Dezaktywuj();
            _unitOfWork.SaveChanges();
        }

        public Uzytkownik? PobierzUzytkownika(int id)
        {
            return _unitOfWork.Uzytkownicy.GetById(id);
        }

        public List<Uzytkownik> PobierzWszystkichUzytkownikow()
        {
            return _unitOfWork.Uzytkownicy.GetAll();
        }

        public List<Uzytkownik> ZnajdzPoNazwisku(string nazwisko)
        {
            if (string.IsNullOrWhiteSpace(nazwisko))
                return new List<Uzytkownik>();

            return _unitOfWork.Uzytkownicy.GetAll()
                .Where(u => u.Nazwisko.Contains(
                    nazwisko, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

/*
 * TODO: UzytkownikService - przyszłe usprawnienia
 *
 * 1. Dodać walidację formatu adresu e-mail,
 *    a nie tylko sprawdzanie, czy pole jest puste.
 *
 * 2. Normalizować adresy e-mail przed porównywaniem
 *    (np. usuwanie zbędnych spacji).
 *
 * 3. Wydzielić wspólną metodę walidacji użytkownika,
 *    aby nie powtarzać tego samego kodu
 *    w DodajUzytkownika() i EdytujUzytkownika().
 *
 * 4. Dodać sprawdzanie poprawności danych:
 *    - długości imienia i nazwiska,
 *    - długości adresu e-mail,
 *    - niedozwolonych wartości.
 *
 * 5. Zabezpieczyć aktualizację użytkownika.
 *    Obecnie GetById() zwraca referencję do obiektu
 *    przechowywanego w MemoryDbContext.
 *    Zmiany existing są wykonywane bezpośrednio
 *    na obiekcie w pamięci.
 *
 * 6. Rozważyć możliwość zmiany roli użytkownika
 *    (Administrator / Prowadzacy).
 *    Uwzględnić dziedziczenie z Uzytkownik.
 *
 * 7. Rozszerzyć wyszukiwanie użytkowników:
 *    - po imieniu,
 *    - po nazwisku,
 *    - po adresie e-mail,
 *    - po identyfikatorze,
 *    - według roli.
 *
 * 8. Rozszerzyć zasady usuwania użytkowników.
 *    Sprawdzać nie tylko rezerwacje,
 *    ale również historię zmian rezerwacji.
 *    W razie potrzeby dezaktywować konto
 *    zamiast usuwać użytkownika.
 *
 * 9. Dodać możliwość ponownej aktywacji użytkownika.
 *
 * 10. Podczas przejścia na PostgreSQL + EF Core:
 *     - skonfigurować dziedziczenie TPH,
 *     - dodać unikalny indeks na adres e-mail,
 *     - wykorzystać Change Tracking,
 *     - rozważyć metody asynchroniczne.
 *
 * 11. Dodać testy jednostkowe:
 *     - dodawanie użytkownika,
 *     - próba dodania istniejącego e-maila,
 *     - edycja danych użytkownika,
 *     - edycja e-maila na już zajęty,
 *     - dezaktywacja użytkownika,
 *     - usunięcie użytkownika z rezerwacjami,
 *     - wyszukiwanie bez rozróżniania wielkości liter.
 */
