namespace TareaZooplanet.Areas.Admin.Models
{
    public class IndexAdminEspeciesViewModel
    {
        public IEnumerable<EspecieModel> Especies { get; set; } = null!;
    }

    public class EspecieModel
    {
        public int Id { get; set; }
        public string Especie { get; set; } = null!;
        public string? NombreEspecies { get; set; }
    }
}
