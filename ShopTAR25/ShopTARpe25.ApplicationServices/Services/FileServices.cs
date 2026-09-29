using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            // kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // meil on vaja teha muutuja nimega uploadsFolder
                string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

                // kui ei ole wwwroot multiple fileUpload directoryt
                if (!Directory.Exists(uploadsFolder))
                {
                    // tee directory wwwrooti alla
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // iga kord kui failid laed üles, siis tehakse see väikesteks tükkideks
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        // domaini teha File to Api

                        FileToApi path = new FileToApi
                        {
                            //tuleb ära mappidaa 
                            // domain ja ??
                        };
                    }
                }
            }
        }
    }
}