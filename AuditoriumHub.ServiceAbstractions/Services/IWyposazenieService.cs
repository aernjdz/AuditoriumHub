
using AuditoriumHub.DataModel.Entities;

namespace AuditoriumHub.ServiceAbstractions.Services
{
     public interface IWyposazenieService
    {
        void DodajWyposazenie(Wyposazenie wyposazenie);
        void EdytujWyposazenie(Wyposazenie wyposazenie);
        void UsunWyposazenie(int wyposazenieId);

        Wyposazenie? PobierzWyposazenie(int wyposazenieId);
        List<Wyposazenie> PobierzWszystkieWyposazenia();

        void DodajWyposazenieDoSali(int salaId, int wyposazenieId, int ilosc);

        void UsunWyposazenieZSali(int salaId, int wyposazenieId);

        List<WyposazenieSali> PobierzWyposazenieSali(int salaId);
    }
}
