namespace TareaZooplanet.Models.ViewModels
{
    public class ClaseViewModel
    {
        public string? NombreClase { get; set; }
        public IEnumerable<EspecieClaseModel> Especies { get; set; } = null!;
    }

    public class EspecieClaseModel
    {
        public int Id { get; set; }
        public string Especie { get; set; } = null!;
    }
}
