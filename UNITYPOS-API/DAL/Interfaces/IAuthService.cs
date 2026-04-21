using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IAuthService
    {
        public object Login(string Email, string Password);
       
    }
}
