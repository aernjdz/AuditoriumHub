using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.ServiceAbstractions.Services;
namespace AuditoriumHub.Services.Memory.Services
{
   public class BudynekService : IBudynekService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudynekService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DodajBudynek(Budynek budynek)
        {
            ArgumentNullException.ThrowIfNull(budynek);

            if (string.IsNullOrWhiteSpace(budynek.Nazwa))
                throw new ArgumentException(
                    "Nazwa budynku nie może być pusta.");

            if (string.IsNullOrWhiteSpace(budynek.Adres))
                throw new ArgumentException(
                    "Adres budynku nie może być pusty.");

            _unitOfWork.Budynki.Add(budynek);
            _unitOfWork.SaveChanges();
        }

        public void EdytujBudynek(Budynek budynek)
        {
            ArgumentNullException.ThrowIfNull(budynek);

            var existing = _unitOfWork.Budynki.GetById(budynek.Id);

            if (existing == null)
                throw new InvalidOperationException(
                    $"Budynek o Id {budynek.Id} nie istnieje.");

            if (string.IsNullOrWhiteSpace(budynek.Nazwa))
                throw new ArgumentException(
                    "Nazwa budynku nie może być pusta.");

            if (string.IsNullOrWhiteSpace(budynek.Adres))
                throw new ArgumentException(
                    "Adres budynku nie może być pusty.");

            existing.Nazwa = budynek.Nazwa;
            existing.Adres = budynek.Adres;

            _unitOfWork.SaveChanges();
        }

        public void UsunBudynek(int budynekId)
        {
            var existing = _unitOfWork.Budynki.GetById(budynekId);

            if (existing == null)
                throw new InvalidOperationException(
                    $"Budynek o Id {budynekId} nie istnieje.");

            bool maSale = _unitOfWork.Sale.GetAll()
                .Any(s => s.BudynekId == budynekId);

            if (maSale)
                throw new InvalidOperationException(
                    "Nie można usunąć budynku, ponieważ posiada sale.");

            _unitOfWork.Budynki.Delete(budynekId);
            _unitOfWork.SaveChanges();
        }

        public Budynek? PobierzBudynek(int budynekId)
        {
            return _unitOfWork.Budynki.GetById(budynekId);
        }

        public List<Budynek> PobierzWszystkieBudynki()
        {
            return _unitOfWork.Budynki.GetAll();
        }

        public List<Sala> PobierzSaleBudynku(int budynekId)
        {
            if (_unitOfWork.Budynki.GetById(budynekId) == null)
                throw new InvalidOperationException(
                    $"Budynek o Id {budynekId} nie istnieje.");

            return _unitOfWork.Sale.GetAll()
                .Where(s => s.BudynekId == budynekId)
                .OrderBy(s => s.Numer)
                .ToList();
        }

    }
}

/*
 * TODO: BudynekService - przyszłe usprawnienia
 *
 * 1. Wydzielić wspólną walidację nazwy i adresu budynku.
 * 2. Dodać sprawdzanie unikalności nazwy budynku.
 * 3. Dodać walidację długości nazwy i adresu.
 * 4. Rozszerzyć wyszukiwanie budynków po nazwie i adresie.
 * 5. Dodać testy usuwania budynku posiadającego sale.
 * 6. Zapewnić spójność relacji Budynek-Sala.
 * 7. Przy przejściu na EF Core skonfigurować
 *    klucze obce i ograniczenie usuwania (DeleteBehavior.Restrict).
 * 8. Rozważyć bezpieczne aktualizowanie encji
 *    zamiast bezpośredniej modyfikacji referencji.
 */
