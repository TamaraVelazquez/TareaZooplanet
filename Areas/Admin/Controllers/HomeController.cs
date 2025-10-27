using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TareaZooplanet.Areas.Admin.Models;
using TareaZooplanet.Services;

namespace TareaZooplanet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public EspeciesServices EspeciesServices { get; }

        public HomeController(EspeciesServices especiesServices)
        {
            EspeciesServices = especiesServices;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(int? id)
        {
            var vm = EspeciesServices.GetEspecies(id);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var vm = EspeciesServices.GetEliminar(id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Eliminar(EliminarAdminEspecieViewModel vm)
        {
            EspeciesServices.Eliminar(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var vm = EspeciesServices.GetEditar(id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(EditarAdminEspecieViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Clases = EspeciesServices.GetAgregar().Clases;
                return View(vm);
            }
            EspeciesServices.Editar(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            var vm = EspeciesServices.GetAgregar();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(AgregarAdminEspecieViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Clases = EspeciesServices.GetAgregar().Clases;
                return View(vm);
            }
            EspeciesServices.Agregar(vm);
            return RedirectToAction("Index");
        }
    }
}
