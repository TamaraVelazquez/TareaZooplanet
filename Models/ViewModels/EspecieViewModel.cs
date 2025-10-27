namespace TareaZooplanet.Models.ViewModels
{
    public class EspecieViewModel
    {
        public int Id { get; set; }
        public string Especie { get; set; } = null!;
        public int? IdClase { get; set; }
        public string? NombreClase { get; set; }
        public string? Habitat { get; set; }
        public double? Peso { get; set; }
        public int? Tamaño { get; set; }
        public string? Observaciones { get; set; }
    }
}
