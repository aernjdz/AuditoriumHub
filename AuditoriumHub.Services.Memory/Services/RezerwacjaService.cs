using AuditoriumHub.ServiceAbstractions.Services;
using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.DataModel.Enums;
using AuditoriumHub.DataModel.Entities;
namespace AuditoriumHub.Services.Memory.Services
{
    public class RezerwacjaService : IRezerwacjaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RezerwacjaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool SprawdzKonflikt(int salaId, DateTime od, DateTime doDaty)
        {
           if (od >= doDaty) throw new ArgumentException(
                    "Nieprawidłowy przedział czasu.");
            return _unitOfWork.Rezerwacje.GetAll().Any(r => 
                r.SalaId == salaId &&
                r.Status == StatusRezerwacji.Aktywna &&
                od < r.DataDo && r.DataOd < doDaty
            );
        }
        public void UtworzRezerwacje(Rezerwacja rezerwacja)
        {
            ArgumentNullException.ThrowIfNull(rezerwacja);
            var sala = _unitOfWork.Sale.GetById(rezerwacja.SalaId);
            if (sala == null) throw new InvalidOperationException(
                $"Sala o Id {rezerwacja.SalaId} nie istnieje.");
            if (!sala.CzyAktywna) throw new InvalidOperationException(
                $"Sala o Id {rezerwacja.SalaId} jest nieaktywna.");
            var uzytkownik = _unitOfWork.Uzytkownicy
    .GetById(rezerwacja.UzytkownikId);

            if (uzytkownik == null)
                throw new InvalidOperationException(
                    "Użytkownik nie istnieje.");

            if (!uzytkownik.CzyAktywny)
                throw new InvalidOperationException(
                    "Użytkownik jest nieaktywny.");
            if (SprawdzKonflikt(rezerwacja.SalaId, rezerwacja.DataOd, rezerwacja.DataDo)) throw new InvalidOperationException(
                $"Sala o Id {rezerwacja.SalaId} jest już zarezerwowana w tym przedziale czasu.");
            rezerwacja.Status = StatusRezerwacji.Aktywna;
            _unitOfWork.Rezerwacje.Add(rezerwacja);
            _unitOfWork.SaveChanges();
        }



        public void EdytujRezerwacje(Rezerwacja rezerwacja)
        {
            ArgumentNullException.ThrowIfNull(rezerwacja);

            var existing = _unitOfWork.Rezerwacje.GetById(rezerwacja.Id);

            if (existing == null)
                throw new InvalidOperationException(
                    "Rezerwacja nie istnieje.");

            if (existing.Status != StatusRezerwacji.Aktywna)
                throw new InvalidOperationException(
                    "Nie można edytować anulowanej rezerwacji.");

            if (rezerwacja.DataOd >= rezerwacja.DataDo)
                throw new ArgumentException(
                    "Nieprawidłowy przedział czasu.");

            
            var sala = _unitOfWork.Sale.GetById(rezerwacja.SalaId);

            if (sala == null)
                throw new InvalidOperationException(
                    "Wybrana sala nie istnieje.");

            if (!sala.CzyAktywna)
                throw new InvalidOperationException(
                    "Wybrana sala jest nieaktywna.");

         
            bool konflikt = _unitOfWork.Rezerwacje.GetAll()
                .Any(r =>
                    r.Id != existing.Id &&
                    r.SalaId == rezerwacja.SalaId &&
                    r.Status == StatusRezerwacji.Aktywna &&
                    rezerwacja.DataOd < r.DataDo &&
                    r.DataOd < rezerwacja.DataDo);

            if (konflikt)
                throw new InvalidOperationException(
                    "Sala jest już zarezerwowana w tym terminie.");

           
            existing.SalaId = rezerwacja.SalaId;
            existing.ZmienTermin(rezerwacja.DataOd, rezerwacja.DataDo);
            existing.Cel = rezerwacja.Cel;

            _unitOfWork.SaveChanges();
        }



        public void AnulujRezerwacje(int rezerwacjaId)
        {
            var existing = _unitOfWork.Rezerwacje.GetById(rezerwacjaId);
            if (existing == null) throw new InvalidOperationException(
                $"Rezerwacja o Id {rezerwacjaId} nie istnieje.");
            if (existing.Status != StatusRezerwacji.Aktywna) throw new InvalidOperationException(
                $"Nie można anulować rezerwacji o Id {rezerwacjaId}, ponieważ nie jest aktywna.");
            existing.Anuluj();
            _unitOfWork.SaveChanges();
        }
        public Rezerwacja? PobierzRezerwacje(int rezerwacjaId)
        {
            return _unitOfWork.Rezerwacje.GetById(rezerwacjaId);
        }
        public List<Rezerwacja> PobierzWszystkieRezerwacje()
        {
            return _unitOfWork.Rezerwacje.GetAll();
        }

    }
}


/*
 * TODO: RezerwacjaService - przyszłe usprawnienia
 *
 * 1. Dodać zapisywanie historii rezerwacji
 *    (utworzenie, zmiana terminu, zmiana sali, anulowanie).
 *
 * 2. Zapisywać użytkownika, który wykonał zmianę,
 *    oraz datę wykonania operacji.
 *
 * 3. Zapewnić spójność danych podczas edycji rezerwacji.
 *    Obecnie GetById() zwraca referencję do obiektu
 *    przechowywanego w MemoryDbContext.
 *    Zmiany w existing są więc zapisywane natychmiast,
 *    nawet bez wywołania Update() lub SaveChanges().
 *
 * 4. Rozważyć mechanizm kopiowania encji lub transakcje,
 *    aby zapobiec częściowemu zapisaniu zmian w razie błędu.
 *
 * 5. Synchronizować SalaId z właściwością nawigacyjną
 *    Sala podczas zmiany sali.
 *
 * 6. Dodać możliwość zmiany użytkownika rezerwacji
 *    (jeżeli pozwalają na to wymagania biznesowe).
 *    Sprawdzać istnienie i aktywność użytkownika.
 *
 * 7. Rozszerzyć walidację danych rezerwacji,
 *    w szczególności celu i poprawności terminów.
 *
 * 8. Podczas przejścia na Entity Framework Core
 *    i PostgreSQL wykorzystać Change Tracking,
 *    SaveChanges() oraz transakcje.
 *
 * 9. Dodać testy jednostkowe:
 *    - zmiana sali na zajętą,
 *    - zmiana sali na nieaktywną,
 *    - edycja anulowanej rezerwacji,
 *    - edycja bez zmiany terminu,
 *    - rezerwacje następujące bezpośrednio po sobie,
 *    - anulowanie rezerwacji,
 *    - brak częściowych zmian po błędzie.
 */
