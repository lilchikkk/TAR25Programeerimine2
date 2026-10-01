using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        Task FilesToApi(SpaceshipDto dto, Spaceship domain);
    }
}
