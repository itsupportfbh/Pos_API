using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.DAL.Services;

namespace UNITYPOS_API.Injectors
{
    public class OrganizationInjector
    {
        public static void InjectInjectors(IServiceCollection services)
        {
            _ = services.AddScoped<IMenuService, MenuService>();
            _ = services.AddScoped<IAuthService, AuthService>();
            _ = services.AddScoped<IOrganizationService, OrganizationService>();
            _ = services.AddScoped<IBranchService, BranchService>();
            _ = services.AddScoped<ICounterService, CounterService>();
            _ = services.AddScoped<ITerminalService, TerminalService>();
            _ = services.AddScoped<IPrinterService, PrinterService>();
            _ = services.AddScoped<IFoodmenu, FoodMenuService>();
            _ = services.AddScoped<IFoodCategory, FoodCategoryService>();
            _ = services.AddScoped<IFoodSubCategory, FoodSubCategoryService>();
            _ = services.AddScoped<ICommonService, CommonService>();
            _ = services.AddScoped<ITax, TaxService>();
            _ = services.AddScoped<IRoleMasterService, RoleMasterService>();
            _ = services.AddScoped<IUserMasterService, UserMasterService>();
            _ = services.AddScoped<IUserBranchMappingService, UserBranchMappingService>();
            _ = services.AddScoped<IUserRoleMappingService, UserRoleMappingService>();
            _ = services.AddScoped<ICustomerService, CustomerService>();
            _ = services.AddScoped<IFloorService, FloorService>();



        }
    }
}
