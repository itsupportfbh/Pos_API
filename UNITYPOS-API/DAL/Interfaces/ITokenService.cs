using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime Expiration) GenerateToken(UserMaster user);
    }
}
