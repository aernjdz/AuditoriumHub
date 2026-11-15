using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
    public class Wyposazenie : IEntity
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;

    }
}
