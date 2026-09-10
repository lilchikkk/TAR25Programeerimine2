using Microsoft.AspNetCore.Mvc;
using ShopTAR25.Models.Spaceship;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;

        //teha konstruktor et saaks kasutada teenust, mis on 
        //defineeritud ISpaceservices liideses
        public SpaceshipController
            (
                ISpaceshipServices  spaceshipServices
            )
        {
            _spaceshipService = spaceshipServices;
        }


        public IActionResult Index()
        {
            return View();
        }

        // kui kasutaja klikkib *created nuppu, siis see meetod käivatatakse
        // tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // kui oled teinud vormi, siis see meeetod käivatatakse
        // saadab andmed servicese, kes on need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                Builddate = vm.Builddate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
            };
            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}
