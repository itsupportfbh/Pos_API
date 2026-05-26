using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.DBLog;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.Common;


namespace UNITYPOS_API.Data.Context
{
    public class POSContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public static string connectionString { get; set; }

        public DbSet<Response> Responses { get; set; }

        public POSContext(
            DbContextOptions<POSContext> options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            var nameClaim = claimsIdentity?.FindFirst("DataBaseName");

            if (nameClaim != null || !string.IsNullOrWhiteSpace(connectionString))
            {
                var dbName = nameClaim != null ? nameClaim.Value : connectionString;
                var baseConnection = _configuration["ConnectionStrings:ConnectionString"];
                var finalConnection = baseConnection?.Replace("_DynamicCustomDB_", dbName);

                if (!string.IsNullOrWhiteSpace(finalConnection))
                    optionsBuilder.UseSqlServer(finalConnection);
            }
            else
            {
                var connection = _configuration["ConnectionStrings:ConnectionString"];

                if (!string.IsNullOrWhiteSpace(connection))
                    optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<System.IO.Stream>();

            modelBuilder.Entity<Response>().HasNoKey();

            modelBuilder.Entity<DBAudit>().ToTable("DBAudit", "dbo");
            modelBuilder.Entity<ErrorLog>().ToTable("ErrorLog", "dbo");
            modelBuilder.Entity<Organization>().ToTable("Organization", "dbo");
            modelBuilder.Entity<Counter>().ToTable("Counter", "dbo");
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster", "dbo");
            modelBuilder.Entity<UserMaster>().ToTable("UserMaster", "dbo");
            modelBuilder.Entity<Branch>().ToTable("Branch", "dbo");
            modelBuilder.Entity<UserBranchMapping>().ToTable("UserBranchMapping", "dbo");
            modelBuilder.Entity<Menu>().ToTable("Menu", "dbo");
            modelBuilder.Entity<SubMenu>().ToTable("SubMenu", "dbo");
            modelBuilder.Entity<RoleMaster>().ToTable("RoleMaster", "dbo");
            modelBuilder.Entity<UserRoleMapping>().ToTable("UserRoleMapping", "dbo");
            modelBuilder.Entity<Terminal>().ToTable("Terminal", "dbo");
            modelBuilder.Entity<Printer>().ToTable("Printer", "dbo");
            modelBuilder.Entity<FoodMenu>().ToTable("FoodMenu", "dbo");
            modelBuilder.Entity<FoodMenuCategory>().ToTable("FoodMenuCategory", "dbo");
            modelBuilder.Entity<FoodMenuSubCategory>().ToTable("FoodMenuSubcategory", "dbo");
            modelBuilder.Entity<StateMaster>().ToTable("StateMaster", "dbo");
            modelBuilder.Entity<CityMaster>().ToTable("CityMaster", "dbo");
            modelBuilder.Entity<Tax>().ToTable("Tax", "dbo");
            modelBuilder.Entity<CustomerMaster>().ToTable("CustomerMaster", "dbo");
            modelBuilder.Entity<FloorMaster>().ToTable("FloorMaster", "dbo");
            modelBuilder.Entity<PaymodeMaster>().ToTable("PaymodeMaster", "dbo");
            modelBuilder.Entity<OrganizationConfig>().ToTable("OrganizationConfig", "dbo");
            modelBuilder.Entity<OrdersHold>().ToTable("OrdersHold", "dbo");
            modelBuilder.Entity<OrderHoldItems>().ToTable("OrderHoldItems", "dbo");
            modelBuilder.Entity<CodeTemplate>().ToTable("CodeTemplate", "dbo");
            modelBuilder.Entity<DiningTableMaster>().ToTable("DiningTableMaster", "dbo");
            modelBuilder.Entity<ComboMenu>().ToTable("ComboMenu", "dbo");
            modelBuilder.Entity<EmployeeMaster>().ToTable("EmployeeMaster", "dbo");
            modelBuilder.Entity<Reservations>().ToTable("Reservations", "dbo");
            modelBuilder.Entity<ReservationTablesMapping>().ToTable("ReservationTablesMapping", "dbo");
            modelBuilder.Entity<Orders>().ToTable("Orders", "dbo");
            modelBuilder.Entity<Orderitems>().ToTable("Orderitems", "dbo");
            modelBuilder.Entity<EntityMaster>().ToTable("EntityMaster", "dbo");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermission", "dbo");
            modelBuilder.Entity<JoinTables>().ToTable("JoinTables", "dbo");
            modelBuilder.Entity<JoinTabledetails>().ToTable("JoinTabledetails", "dbo");
        }
    }
}