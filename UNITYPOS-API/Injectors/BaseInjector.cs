using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;

namespace UNITYPOS_API.Injectors
{
    public class BaseInjector
    {

        public static void InjectInjectors(IServiceCollection services)
        {
            _ = services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _ = services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            _ = services.AddScoped<IUnitOfWork, UnitOfWork>();



            //

            OrganizationInjector.InjectInjectors(services);


        }
    }
}
