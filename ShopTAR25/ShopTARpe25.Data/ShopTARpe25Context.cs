using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;


namespace ShopTARpe25.Data
{
<<<<<<< Updated upstream
    //teha sellest classi DbContext, et saaks andmebaasi kasutada
    public class ShopTARpe25Context : DbContext 
=======
    // teeme sellest DbContext'i, et saaks andmebaasi kasutada
    public class ShopTARpe25Context : DbContext
>>>>>>> Stashed changes
    {
        public ShopTARpe25Context(DbContextOptions<ShopTARpe25Context> options)
            : base (options)
        {
        }
<<<<<<< Updated upstream
        //teha DbSet, et saaks andmebaasi kasutada
        //nimega SpaceShip
=======

>>>>>>> Stashed changes
        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
    }
}
