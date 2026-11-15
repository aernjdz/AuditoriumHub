using AuditoriumHub.DataModel.Entities;

namespace AuditoriumHub.ServiceAbstractions.Services
{
    public interface IRezerwacjaService
    {
        void UtworzRezerwacje(Rezerwacja rezerwacja);
        void EdytujRezerwacje(Rezerwacja rezerwacja);
        void AnulujRezerwacje(int rezerwacjaId);

        Rezerwacja? PobierzRezerwacje(int rezerwacjaId);
        List<Rezerwacja> PobierzWszystkieRezerwacje();

        bool SprawdzKonflikt(
            int salaId,
            DateTime od,
            DateTime doDaty);

    }
}
