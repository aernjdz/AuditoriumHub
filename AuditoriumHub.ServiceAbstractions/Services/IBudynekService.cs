using AuditoriumHub.DataModel.Entities;

namespace AuditoriumHub.ServiceAbstractions.Services
{
    public interface IBudynekService
    {
        void DodajBudynek(Budynek budynek);
        void EdytujBudynek(Budynek budynek);
        void UsunBudynek(int budynekId);

        Budynek? PobierzBudynek(int budynekId);
        List<Budynek> PobierzWszystkieBudynki();

        List<Sala> PobierzSaleBudynku(int budynekId);
    }
}
