using AuditoriumHub.DataModel.Entities.Users;
namespace AuditoriumHub.ServiceAbstractions.Services
{
    public interface IUzytkownikService
    {
        void EdytujUzytkownika(Uzytkownik uzytkownik);
        void UsunUzytkownika(int id);
        void DezaktywujUzytkownika(int id);

        Uzytkownik? PobierzUzytkownika(int id);
        List<Uzytkownik> PobierzWszystkichUzytkownikow();

        List<Uzytkownik> ZnajdzPoNazwisku(string nazwisko);
    }
}
