using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private const string UploadFolderName = "multipleFileUpload";

        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

<<<<<<< Updated upstream

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
=======
        public FileServices(
            ShopTARpe25Context context,
            IHostEnvironment webHost)
>>>>>>> Stashed changes
        {
            _context = context;
            _webHost = webHost;
        }

<<<<<<< Updated upstream

        public async Task FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            //kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kui ei ole wwroot multiple fileUpload directoryt
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //tee directory wwwrooti alla
                    Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");

                    foreach (var file in dto.Files)
                    {
                        //meil on vaja teha muutuja nimega uploadsFolder
                        //sinna muutuja taha on vaja Path kombineeria
                        string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                        //igale failile unikaalne Guid selle nime ette
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);

                            //domaini teha FileToApi
                            FileToApi path = new FileToApi
                            {
                                //tuleb ära mappida
                                //domain ja ??
                                Id = Guid.NewGuid(),
                                ExistingFilePath = uniqueFileName,
                                SpaceshipId = domain.Id
                            };

                            await _context.FileToApis.AddAsync(path);
                        }
                    }
=======
        private string UploadsFolder =>
            Path.Combine(_webHost.ContentRootPath, "wwwroot", UploadFolderName);

        public async Task FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files == null || dto.Files.Count == 0)
            {
                return;
            }

            // loo kaust, kui seda veel ei ole (CreateDirectory ei viska viga, kui kaust on olemas)
            Directory.CreateDirectory(UploadsFolder);

            foreach (var file in dto.Files)
            {
                if (file == null || file.Length == 0)
                {
                    continue;
                }

                // igale failile unikaalne Guid nime ette;
                // Path.GetFileName kaitseb path traversal'i eest
                string uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);

                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                FileToApi path = new()
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    SpaceshipId = domain.Id
                };

                await _context.FileToApis.AddAsync(path);
            }
        }

        public async Task RemoveFilesBySpaceshipId(Guid spaceshipId)
        {
            var files = await _context.FileToApis
                .Where(x => x.SpaceshipId == spaceshipId)
                .ToListAsync();

            foreach (var file in files)
            {
                var filePath = Path.Combine(UploadsFolder, file.ExistingFilePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
>>>>>>> Stashed changes
                }
                
            }

            _context.FileToApis.RemoveRange(files);
        }
    }
}
