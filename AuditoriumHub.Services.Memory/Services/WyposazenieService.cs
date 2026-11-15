
using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.ServiceAbstractions.Services;

namespace AuditoriumHub.Services.Memory.Services
{
    public class WyposazenieService : IWyposazenieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WyposazenieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DodajWyposazenie(Wyposazenie wyposazenie)
        {
            ArgumentNullException.ThrowIfNull(wyposazenie);

            if (string.IsNullOrWhiteSpace(wyposazenie.Nazwa))
                throw new ArgumentException(
                    "Nazwa wyposażenia nie może być pusta.");

            _unitOfWork.Wyposazenia.Add(wyposazenie);
            _unitOfWork.SaveChanges();
        }

        public void EdytujWyposazenie(Wyposazenie wyposazenie)
        {
            ArgumentNullException.ThrowIfNull(wyposazenie);

            var existing = _unitOfWork.Wyposazenia
                .GetById(wyposazenie.Id);

            if (existing == null)
                throw new InvalidOperationException(
                    "Wyposażenie nie istnieje.");

            if (string.IsNullOrWhiteSpace(wyposazenie.Nazwa))
                throw new ArgumentException(
                    "Nazwa wyposażenia nie może być pusta.");

            existing.Nazwa = wyposazenie.Nazwa;
            existing.Opis = wyposazenie.Opis;

            _unitOfWork.SaveChanges();
        }

        public void UsunWyposazenie(int wyposazenieId)
        {
            bool jestPrzypisane = _unitOfWork.WyposazenieSali
                .GetAll()
                .Any(w => w.WyposazenieId == wyposazenieId);

            if (jestPrzypisane)
                throw new InvalidOperationException(
                    "Nie można usunąć wyposażenia przypisanego do sal.");

            _unitOfWork.Wyposazenia.Delete(wyposazenieId);
            _unitOfWork.SaveChanges();
        }

        public Wyposazenie? PobierzWyposazenie(int wyposazenieId)
        {
            return _unitOfWork.Wyposazenia.GetById(wyposazenieId);
        }

        public List<Wyposazenie> PobierzWszystkieWyposazenia()
        {
            return _unitOfWork.Wyposazenia.GetAll();
        }

        public void DodajWyposazenieDoSali(
            int salaId, int wyposazenieId, int ilosc)
        {
            if (_unitOfWork.Sale.GetById(salaId) == null)
                throw new InvalidOperationException(
                    "Sala nie istnieje.");

            if (_unitOfWork.Wyposazenia.GetById(wyposazenieId) == null)
                throw new InvalidOperationException(
                    "Wyposażenie nie istnieje.");

            if (ilosc <= 0)
                throw new ArgumentException(
                    "Ilość musi być większa od zera.");

            bool istnieje = _unitOfWork.WyposazenieSali.GetAll()
                .Any(w => w.SalaId == salaId &&
                          w.WyposazenieId == wyposazenieId);

            if (istnieje)
                throw new InvalidOperationException(
                    "Wyposażenie jest już przypisane do tej sali.");

            int noweId = _unitOfWork.WyposazenieSali.GetAll()
                .Select(w => w.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var przypisanie = new WyposazenieSali
            {
                Id = noweId,
                SalaId = salaId,
                WyposazenieId = wyposazenieId,
                Ilosc = ilosc
            };

            _unitOfWork.WyposazenieSali.Add(przypisanie);
            _unitOfWork.SaveChanges();
        }

        public void UsunWyposazenieZSali(
            int salaId, int wyposazenieId)
        {
            var przypisanie = _unitOfWork.WyposazenieSali.GetAll()
                .FirstOrDefault(w =>
                    w.SalaId == salaId &&
                    w.WyposazenieId == wyposazenieId);

            if (przypisanie == null)
                throw new InvalidOperationException(
                    "Wyposażenie nie jest przypisane do sali.");

            _unitOfWork.WyposazenieSali.Delete(przypisanie.Id);
            _unitOfWork.SaveChanges();
        }

        public List<WyposazenieSali> PobierzWyposazenieSali(int salaId)
        {
            if (_unitOfWork.Sale.GetById(salaId) == null)
                throw new InvalidOperationException(
                    "Sala nie istnieje.");

            return _unitOfWork.WyposazenieSali.GetAll()
                .Where(w => w.SalaId == salaId)
                .ToList();
        }
    }
}

/*
 * TODO: WyposazenieService - przyszłe usprawnienia
 *
 * 1. Dodać możliwość zmiany ilości wyposażenia w sali.
 * 2. Wydzielić wspólną walidację wyposażenia.
 * 3. Dodać walidację unikalności nazwy wyposażenia.
 * 4. Dodać wyszukiwanie sal po wymaganym wyposażeniu.
 * 5. Dodać testy przypisywania i usuwania wyposażenia.
 * 6. Zapewnić spójność właściwości nawigacyjnych
 *    Sala, Wyposazenie oraz WyposazenieSali.
 * 7. Zastąpić ręczne generowanie Id mechanizmem
 *    automatycznego generowania kluczy.
 * 8. Przy przejściu na EF Core skonfigurować
 *    relacje i ograniczenia unikalności
 *    dla pary (SalaId, WyposazenieId).
 */
