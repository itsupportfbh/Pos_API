using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;

namespace UNITYPOS_API.Injectors
{
    public class OrganizationInjector
    {
        public static void InjectInjectors(IServiceCollection services)
        {
            _ = services.AddScoped<IOrganizationservice, OrganizationService>();
            _ = services.AddScoped<ICounterService, CounterService>();
            _ = services.AddScoped<IBranchService, BranchService>();





        }
    }
}
