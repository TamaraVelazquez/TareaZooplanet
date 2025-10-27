using TareaZooplanet.Models.Entities;
using TareaZooplanet.Repositories;

namespace TareaZooplanet.Services
{
    public class ClasesServices
    {
        public ClasesServices(Repository<Clase> clasesRepository)
        {
            ClasesRepository = clasesRepository;
        }

        public Repository<Clase> ClasesRepository { get; }

        public IEnumerable<string> GetClases()
        {
            return ClasesRepository.GetAll().Select(x => x.Nombre ?? "").OrderBy(x => x);
        }
    }
}
