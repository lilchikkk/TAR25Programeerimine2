using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;
        private readonly ShopTARpe25Context _context;
        private readonly IWebHostEnvironment _webHost;

        public SpaceshipController(
            ISpaceshipServices spaceshipServices,
            ShopTARpe25Context context,
            IWebHostEnvironment webHost)
        {
            _spaceshipServices = spaceshipServices;
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var result = await _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    Builddate = x.Builddate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                })
                .ToListAsync();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SpaceshipCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                Builddate = vm.Builddate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            var result = await _spaceshipServices.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDetailsViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                Classification = spaceship.Classification,
                Builddate = spaceship.Builddate,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower,
                CreatedAt = spaceship.CreatedAt,
                ModifiedAt = spaceship.ModifiedAt
            };

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    ImageId = y.Id,
                    FilePath = y.ExistingFilePath,
                    SpaceshipId = y.SpaceshipId
                })
                .ToArrayAsync();

            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipUpdateViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                Classification = spaceship.Classification,
                Builddate = spaceship.Builddate,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower,
                CreatedAt = spaceship.CreatedAt,
                ModifiedAt = spaceship.ModifiedAt
            };

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    ImageId = y.Id,
                    FilePath = y.ExistingFilePath,
                    SpaceshipId = y.SpaceshipId
                })
                .ToArrayAsync();

            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var existingImages = await _context.FileToApis
                    .Where(x => x.SpaceshipId == vm.Id)
                    .Select(y => new ImageViewModel
                    {
                        ImageId = y.Id,
                        FilePath = y.ExistingFilePath,
                        SpaceshipId = y.SpaceshipId
                    })
                    .ToArrayAsync();

                vm.Image.AddRange(existingImages);

                return View(vm);
            }

            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                Builddate = vm.Builddate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt, 
                Files = vm.Files            
            };

            var result = await _spaceshipServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.FileToApis.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Update), new { id = spaceshipId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }


            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.Builddate = spaceship.Builddate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var spaceship = await _spaceshipServices.Delete(id);

             if (spaceship == null)
             {
                return RedirectToAction(nameof(Index));
             }

             return RedirectToAction(nameof(Index));
        }
    }
}