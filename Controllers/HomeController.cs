using Microsoft.AspNetCore.Mvc;
using TareaZooplanet.Models.Entities;
using TareaZooplanet.Models.ViewModels;
using TareaZooplanet.Services;

namespace TareaZooplanet.Controllers
{
    public class HomeController : Controller
    {
        private readonly EspeciesServices especiesServices;

        public HomeController(EspeciesServices especiesServices)
        {
            this.especiesServices = especiesServices;
        }
        public IActionResult Index()
        {
            AnimalesContext context = new AnimalesContext();
            IndexViewModel vm = new IndexViewModel();
            vm.Clases = context.Clase.OrderBy(x => x.Nombre)
                .Select(x => new ClasesModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion
                });
            return View(vm);
        }

        public IActionResult Clase(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var vm = especiesServices.GetEspeciesByClases(id);
            return View(vm);
        }

        public IActionResult Detalle(int id)
        {
            var vm = especiesServices.GetEspecie(id);
            if (vm != null)
            {
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
    }
}
