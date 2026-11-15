using AuditoriumHub.DataModel.Entities;

namespace AuditoriumHub.ServiceAbstractions.Services
{
    public interface IHarmonogramService
    {
        List<Rezerwacja> PobierzDzienny(DateTime data);

        List<Rezerwacja> PobierzTygodniowy(DateTime data);

        List<Rezerwacja> PobierzDlaSali(int salaId);

        List<Rezerwacja> PobierzDlaUzytkownika(int uzytkownikId);
    }
}
