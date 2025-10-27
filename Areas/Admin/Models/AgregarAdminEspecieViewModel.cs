using TareaZooplanet.Models.Entities;

namespace TareaZooplanet.Areas.Admin.Models
{
    public class AgregarAdminEspecieViewModel
    {
        public int Id { get; set; }
        public string Especie { get; set; } = null!;
        public IEnumerable<ClaseModel>? Clases { get; set; }
        public int? IdClase { get; set; }
        public string? NombreClase { get; set; }
        public string? Habitat { get; set; }
        public double? Peso { get; set; }
        public int? Tamaño { get; set; }
        public IFormFile? Imagen { get; set; }
        public string? Observaciones { get; set; }
    }
    public class ClaseModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }
}
