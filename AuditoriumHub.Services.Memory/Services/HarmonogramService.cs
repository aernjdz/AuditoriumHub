
using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.DataModel.Enums;
using AuditoriumHub.ServiceAbstractions.Repositories;
using AuditoriumHub.ServiceAbstractions.Services;

namespace AuditoriumHub.Services.Memory.Services
{
  public class HarmonogramService : IHarmonogramService
  {
        private readonly IUnitOfWork _unitOfWork;

        public HarmonogramService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Rezerwacja> PobierzDzienny(DateTime data)
        {
            DateTime poczatek = data.Date;
            DateTime koniec = poczatek.AddDays(1);

            return _unitOfWork.Rezerwacje.GetAll()
                .Where(r => r.Status == StatusRezerwacji.Aktywna)
                .Where(r => r.DataOd < koniec &&
                            r.DataDo > poczatek)
                .OrderBy(r => r.DataOd)
                .ToList();
        }

        public List<Rezerwacja> PobierzTygodniowy(DateTime data)
        {
            int dniOdPoniedzialku =
                ((int)data.DayOfWeek + 6) % 7;

            DateTime poczatek =
                data.Date.AddDays(-dniOdPoniedzialku);

            DateTime koniec = poczatek.AddDays(7);

            return _unitOfWork.Rezerwacje.GetAll()
                .Where(r => r.Status == StatusRezerwacji.Aktywna)
                .Where(r => r.DataOd < koniec &&
                            r.DataDo > poczatek)
                .OrderBy(r => r.DataOd)
                .ToList();
        }

        public List<Rezerwacja> PobierzDlaSali(int salaId)
        {
            return _unitOfWork.Rezerwacje.GetAll()
                .Where(r => r.SalaId == salaId &&
                            r.Status == StatusRezerwacji.Aktywna)
                .OrderBy(r => r.DataOd)
                .ToList();
        }

        public List<Rezerwacja> PobierzDlaUzytkownika(int uzytkownikId)
        {
            return _unitOfWork.Rezerwacje.GetAll()
                .Where(r => r.UzytkownikId == uzytkownikId &&
                            r.Status == StatusRezerwacji.Aktywna)
                .OrderBy(r => r.DataOd)
                .ToList();
        }
    }
}

/*
 * TODO: HarmonogramService - przyszłe usprawnienia
 *
 * 1. Dodać filtrowanie harmonogramu według budynku.
 * 2. Dodać pobieranie harmonogramu konkretnej sali
 *    w wybranym dniu lub tygodniu.
 * 3. Dodać możliwość uwzględniania anulowanych rezerwacji
 *    w widoku historii.
 * 4. Dodać paginację i sortowanie wyników.
 * 5. Dodać testy rezerwacji przechodzących przez północ
 *    oraz granicę tygodnia.
 * 6. Podczas przejścia na EF Core przenieść filtrowanie
 *    do zapytań wykonywanych przez bazę danych.
 */
