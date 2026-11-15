using AuditoriumHub.DataModel.Interfaces;

namespace AuditoriumHub.DataModel.Entities
{
    public class Budynek : IEntity
    {
        public int Id { get;  set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;

    }
}
