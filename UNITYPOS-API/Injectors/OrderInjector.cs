using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.Injectors
{
    public class OrderInjector
    {
        public static void InjectInjectors(IServiceCollection services)
        {
            _ = services.AddScoped<IOrderHoldService, OrderHoldService>();
            //_ = services.AddScoped<IOrderHoldItemsService, OrderHoldService>();

        }
    }
}
