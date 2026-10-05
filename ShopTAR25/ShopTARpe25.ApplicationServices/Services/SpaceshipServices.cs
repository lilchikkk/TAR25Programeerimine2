using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public SpaceshipServices(
            ShopTARpe25Context context,
            IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship domain = new();

            domain.Id = Guid.NewGuid();
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.Builddate = dto.Builddate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;
            //reame saama file teenuses välja kutsuda meetod,
            //mis salvestab failid servisesse
            _fileServices.FilesToApi(dto, domain);

            // salvestame failid ja lisame FileToApi kirjed contexti
            await _fileServices.FilesToApi(dto, domain);

            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Spaceship> DetailsAsync(Guid id)
        {
            var domain = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return domain!;
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            var domain = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
            {
                return null!;
            }

            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.Builddate = dto.Builddate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.ModifiedAt = DateTime.Now;

            await _fileServices.FilesToApi(dto, domain);

            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Spaceship> Delete(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null!;
            }

            // kustutame ka laevaga seotud pildid
            await _fileServices.RemoveFilesBySpaceshipId(id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}