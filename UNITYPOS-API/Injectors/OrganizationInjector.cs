using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;

namespace UNITYPOS_API.Injectors
{
    public class OrganizationInjector
    {
        public static void InjectInjectors(IServiceCollection services)
        {
            _ = services.AddScoped<IMenuService, MenuService>();
            _ = services.AddScoped<IOrganizationservice, OrganizationService>();
            _ = services.AddScoped<IBranchService, BranchService>();
            _ = services.AddScoped<ICounterService, CounterService>();
            _ = services.AddScoped<ITerminalService, TerminalService>();
            _ = services.AddScoped<IPrinterService, PrinterService>();
            _ = services.AddScoped<IFoodmenu, FoodMenuService>();
            _ = services.AddScoped<IFoodCategory, FoodCategoryService>();
            _ = services.AddScoped<IFoodSubCategory, FoodSubCategoryService>();





        }
    }
}
