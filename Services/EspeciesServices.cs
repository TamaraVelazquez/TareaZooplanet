using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TareaZooplanet.Areas.Admin.Models;
using TareaZooplanet.Models.Entities;
using TareaZooplanet.Models.ViewModels;
using TareaZooplanet.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TareaZooplanet.Services
{
    public class EspeciesServices
    {
        public EspeciesServices(Repository<Especies> especiesRepository, Repository<Clase> clasesRepository, IWebHostEnvironment hostEnvironment)
        {
            EspeciesRepository = especiesRepository;
            ClasesRepository = clasesRepository;
            HostEnvironment = hostEnvironment;
        }
        
        public Repository<Especies> EspeciesRepository { get; }
        public Repository<Clase> ClasesRepository { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public ClaseViewModel GetEspeciesByClases(string nombre)
        {
            ClaseViewModel vm = new();
            vm.NombreClase = nombre;
            vm.Especies = EspeciesRepository.GetAll().AsQueryable()
                .Where(x => x.IdClaseNavigation != null && x.IdClaseNavigation.Nombre == nombre).OrderBy(x => x.Especie)
                .Select(x => new EspecieClaseModel
                {
                    Id = x.Id,
                    Especie = x.Especie
                });
            return vm;
        }

        public EspecieViewModel? GetEspecie(int id)
        {
            EspecieViewModel? vm = EspeciesRepository.GetAll().AsQueryable().Include(x => x.IdClaseNavigation)
                .Where(x => x.Id == id).Select(x => new EspecieViewModel
                {
                    Id = x.Id,
                    Especie = x.Especie,
                    IdClase = x.IdClase,
                    NombreClase = x.IdClaseNavigation.Nombre,
                    Habitat = x.Habitat,
                    Peso = x.Peso,
                    Tamaño = x.Tamaño,
                    Observaciones = x.Observaciones
                }).FirstOrDefault();
            return vm;
        }

        public IndexAdminEspeciesViewModel GetEspecies(int? idClase)
        {
            IndexAdminEspeciesViewModel vm = new();
            vm.Especies = EspeciesRepository.GetAll().AsQueryable().Include(x => x.IdClaseNavigation)
                .Where(x => idClase == null || idClase == x.IdClase).OrderBy(x => x.Especie)
                .Select(x => new EspecieModel
                {
                    Id = x.Id,
                    Especie = x.Especie,
                    NombreEspecies = x.IdClaseNavigation.Nombre ?? "Sin clase"
                });
            return vm;
        }

        public EliminarAdminEspecieViewModel GetEliminar(int id)
        {
            var vm = EspeciesRepository.GetAll().AsQueryable().Include(x => x.IdClaseNavigation).Where(x => x.Id == id)
                .Select(x => new EliminarAdminEspecieViewModel
                {
                    Id = x.Id,
                    Especie = x.Especie,
                    NombreEspecies = x.IdClaseNavigation.Nombre ?? "Sin clase"
                }).FirstOrDefault();
            if (vm == null)
            {
                throw new ArgumentException("Especie no encontrada", nameof(id));
            }

            return vm;
        }

        public void Eliminar(int id)
        {
            var e = EspeciesRepository.Get(id);
            if(e == null)
            {
                throw new ArgumentException("Especie no encontrada", nameof(id));
            }
            EspeciesRepository.Delete(e.Id);
            var img = Path.Combine(HostEnvironment.WebRootPath, "especies", $"{id}.jpg");
            if (File.Exists(img))
            {
                File.Delete(img);
            }
        }

        public EditarAdminEspecieViewModel GetEditar(int id)
        {
            var vm = EspeciesRepository.GetAll().AsQueryable().Include(x => x.IdClaseNavigation).Where(x => x.Id == id)
                .Select(x => new EditarAdminEspecieViewModel
                {
                    Id = x.Id,
                    Especie = x.Especie,
                    IdClase = x.IdClase,
                    NombreClase = x.IdClaseNavigation.Nombre,
                    Habitat = x.Habitat,
                    Peso = x.Peso,
                    Tamaño = x.Tamaño,
                    Clases = ClasesRepository.GetAll()
                    .Select(c => new ClaseModel
                    {
                        Id = c.Id,
                        Nombre = c.Nombre
                    }),
                    Observaciones = x.Observaciones
                }).FirstOrDefault();
            if (vm == null)
            {
                throw new ArgumentException("Especie no encontrada");
            }
            return vm;
        }

        public void Editar(EditarAdminEspecieViewModel vm)
        {
            var e = EspeciesRepository.Get(vm.Id);
            if (e == null)
            {
                throw new ArgumentException("Especie no encontrada");
            }
            e.Especie = vm.Especie;
            e.IdClase = vm.IdClase;
            e.Habitat = vm.Habitat;
            e.Peso = vm.Peso;
            e.Tamaño = vm.Tamaño;
            e.Observaciones = vm.Observaciones;
            EspeciesRepository.Update(e);
            if(vm.Imagen != null)
            {
                AgregarImagen(vm.Imagen, e.Id);
            }
        }

        public void AgregarImagen(IFormFile archivo, int idEspecie)
        {
            if (archivo.Length > 1024 * 1024 * 2)
            {
                throw new ArgumentException("Seleccione una imagen de 2MB o menos.");
            }

            if (archivo.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Selecciones una imagen JPEG o JPG");
            }

            var ruta = HostEnvironment.WebRootPath + $"/especies/{idEspecie}.jpg";
            Directory.CreateDirectory("wwwroot/img_frutas/");
            var file = File.Create(ruta);
            archivo.CopyTo(file);
            file.Close();
        }

        public AgregarAdminEspecieViewModel GetAgregar()
        {
            return new AgregarAdminEspecieViewModel
            {
                Clases = ClasesRepository.GetAll()
                    .Select(c => new ClaseModel 
                    { 
                        Id = c.Id, 
                        Nombre = c.Nombre
                    })
            };
        }

        public void Agregar(AgregarAdminEspecieViewModel vm)
        {
            var e = new Especies
            {
                Id = 0,
                Especie = vm.Especie,
                IdClase = vm.IdClase,
                Habitat = vm.Habitat,
                Peso = vm.Peso,
                Tamaño = vm.Tamaño,
                Observaciones = vm.Observaciones
            };
            EspeciesRepository.Insert(e);
            var id = e.Id;
            if (vm.Imagen != null)
            {
                AgregarImagen(vm.Imagen, id);
            }
            else
            {
                UsarImagenPorDefecto(id);
            }
        }

        private void UsarImagenPorDefecto(int idEspecie)
        {
            var rutaDefecto = Path.Combine(HostEnvironment.WebRootPath, "especies", "no-disponible.png");
            var rutaDestino = Path.Combine(HostEnvironment.WebRootPath, "especies", $"{idEspecie}.jpg");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaDestino)!);

            if (File.Exists(rutaDefecto))
            {
                File.Copy(rutaDefecto, rutaDestino, true);
            }
        }

    }
}
