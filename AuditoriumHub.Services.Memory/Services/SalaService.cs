using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.ServiceAbstractions.Services;
using AuditoriumHub.DataModel.Enums;


namespace AuditoriumHub.Services.Memory.Services
{
    public class SalaService : ISalaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DodajSale(Sala sala)
        {
            if (_unitOfWork.Budynki.GetById(sala.BudynekId) == null)
                throw new InvalidOperationException(
                    "Budynek o podanym ID nie istnieje.");
            if (string.IsNullOrWhiteSpace(sala.Numer))
                throw new ArgumentException(
                    "Numer sali nie może być pusty.");
            if (sala.Pojemnosc <= 0) throw new ArgumentException("Pojemność sali musi być większa od zera.");
            _unitOfWork.Sale.Add(sala);
            _unitOfWork.SaveChanges();
        }

        public void EdytujSale(Sala sala)
        {
            if (_unitOfWork.Budynki.GetById(sala.BudynekId) == null)
                throw new InvalidOperationException(
                    "Budynek o podanym ID nie istnieje.");
            if (string.IsNullOrWhiteSpace(sala.Numer))
                throw new ArgumentException(
                    "Numer sali nie może być pusty.");
            if (sala.Pojemnosc <= 0) throw new ArgumentException("Pojemność sali musi być większa od zera.");
            _unitOfWork.Sale.Update(sala);
            _unitOfWork.SaveChanges();
        }

        

        public void UsunSale(int salaId)
        {
            bool maAktywneRezerwacje =
         _unitOfWork.Rezerwacje.GetAll()
             .Any(r => r.SalaId == salaId &&
                       r.Status == StatusRezerwacji.Aktywna);

            if (maAktywneRezerwacje)
                throw new InvalidOperationException(
                    "Nie można usunąć sali z aktywnymi rezerwacjami.");

            _unitOfWork.Sale.Delete(salaId);
            _unitOfWork.SaveChanges();
        }

        public Sala? PobierzSale(int salaId)
        {
            return _unitOfWork.Sale.GetById(salaId);
        }

        public List<Sala> PobierzWszystkieSale()
        {
            return _unitOfWork.Sale.GetAll();
        }

        public List<Sala> ZnajdzWolneSale(DateTime od, DateTime doDaty, int pojemnosc)
        {
            if(od >= doDaty) throw new ArgumentException("Data początkowa musi być wcześniejsza niż data końcowa.");

            if(pojemnosc <= 0) throw new ArgumentException("Pojemność sali musi być większa od zera.");

            var rezerwacje = _unitOfWork.Rezerwacje.GetAll();

            return _unitOfWork.Sale.GetAll()
                .Where(s => s.CzyAktywna)
                .Where(s => s.Pojemnosc >= pojemnosc)
                .Where(s => !rezerwacje.Any(r =>
                  r.SalaId == s.Id &&
                    r.Status == StatusRezerwacji.Aktywna &&
                    od < r.DataDo &&
                    r.DataOd < doDaty

                )).ToList();

        }
    }
}

/*
 * TODO: SalaService - przyszłe usprawnienia
 *
 * 1. Dodać ArgumentNullException.ThrowIfNull(sala)
 *    w metodach DodajSale() i EdytujSale().
 *
 * 2. Sprawdzać unikalność numeru sali w obrębie budynku.
 *    Dwie sale nie mogą mieć tego samego numeru
 *    w tym samym budynku.
 *    Podczas edycji pomijać salę o aktualnym Id.
 *
 * 3. Wydzielić wspólną walidację sali do osobnej
 *    metody, aby uniknąć powtarzania kodu.
 *
 * 4. Rozszerzyć walidację:
 *    - maksymalna długość numeru sali,
 *    - poprawność pojemności,
 *    - poprawność BudynekId.
 *
 * 5. Zabezpieczyć EdytujSale() przed zmianami,
 *    które mogą naruszyć istniejące rezerwacje.
 *    Sprawdzać np. zmianę budynku i dezaktywację sali.
 *    Określić zasady zmniejszania pojemności.
 *
 * 6. Rozszerzyć UsunSale().
 *    Obecnie sprawdzane są tylko aktywne rezerwacje.
 *    Należy uwzględnić również anulowane rezerwacje,
 *    historię rezerwacji i przypisane wyposażenie.
 *    Rozważyć dezaktywację zamiast usuwania sali.
 *
 * 7. Zapewnić spójność relacji:
 *    - Sala.BudynekId i Sala.Budynek,
 *    - Budynek.Sale,
 *    - Sala.Rezerwacje,
 *    - Sala.WyposazenieSal.
 *
 * 8. Rozszerzyć ZnajdzWolneSale() o filtrowanie:
 *    - według budynku,
 *    - według wymaganego wyposażenia,
 *    - według dostępności wyposażenia
 *      w określonej ilości.
 *
 * 9. Dodać wyszukiwanie sal według numeru,
 *    budynku, pojemności i statusu aktywności.
 *
 * 10. Rozważyć bezpieczniejszy mechanizm aktualizacji.
 *     Obecnie RepositoryMemory<T> przechowuje
 *     referencje do obiektów, więc mogą być one
 *     modyfikowane bez wywołania Update().
 *
 * 11. Podczas przejścia na PostgreSQL + EF Core:
 *     - skonfigurować relacje i klucze obce,
 *     - dodać unikalny indeks na (BudynekId, Numer),
 *     - skonfigurować zasady usuwania powiązanych danych,
 *     - wykorzystać Change Tracking i transakcje,
 *     - rozważyć metody asynchroniczne.
 *
 * 12. Dodać testy jednostkowe:
 *     - dodawanie sali do nieistniejącego budynku,
 *     - dodawanie sali z pustym numerem,
 *     - dodawanie sali z nieprawidłową pojemnością,
 *     - dodawanie dwóch sal o tym samym numerze,
 *     - edycja sali z aktywnymi rezerwacjami,
 *     - usuwanie sali z aktywną rezerwacją,
 *     - usuwanie sali z historią rezerwacji,
 *     - wyszukiwanie wolnych sal,
 *     - pomijanie anulowanych rezerwacji,
 *     - pomijanie nieaktywnych sal,
 *     - rezerwacje z sąsiadującymi terminami,
 *     - rezerwacje z częściowo nakładającymi
 *       się przedziałami czasu.
 */
