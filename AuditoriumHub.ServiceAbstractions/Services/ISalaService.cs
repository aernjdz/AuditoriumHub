using AuditoriumHub.DataModel.Entities;

namespace AuditoriumHub.ServiceAbstractions.Services
{
    public interface ISalaService
    {
        void DodajSale(Sala sala);
        void EdytujSale(Sala sala);
        void UsunSale(int salaId);

        Sala? PobierzSale(int salaId);
        List<Sala> PobierzWszystkieSale();

        List<Sala> ZnajdzWolneSale(
            DateTime od,
            DateTime doDaty,
            int pojemnosc);
    }
}
