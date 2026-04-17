using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.DBLog;

namespace UNITYPOS_API.Data.Context
{
    public class POSContext:DbContext
    {

        //public static string ConnectionString { get; set; }
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IConfiguration _configuration;
        public static string connectionString { get; set; }
        public POSContext(DbContextOptions<POSContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            Claim? nameClaim = claimsIdentity?.FindFirst("DataBaseName");
            if (nameClaim != null || connectionString != null)
            {
                string dbName = nameClaim != null ? nameClaim.Value : null ?? connectionString;
                string? ConnectionString = _configuration["ConnectionStrings:ConnectionString"];
                string? con = ConnectionString?.Replace("_DynamicCustomDB_", dbName);
                if (!string.IsNullOrEmpty(con))
                {
                    _ = optionsBuilder.UseSqlServer(con);
                }
            }
            else
            {
                string? ConnectionString = _configuration["ConnectionStrings:ConnectionString"];
                if (!string.IsNullOrEmpty(ConnectionString))
                {
                    _ = optionsBuilder.UseSqlServer(ConnectionString);
                }
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<RolesMaster>().ToTable("RolesMaster", "master");

                       
            modelBuilder.Entity<DBAudit>().ToTable("DBAudit", "dbo");
            modelBuilder.Entity<ErrorLog>().ToTable("ErrorLog", "dbo");
            modelBuilder.Entity<Organization>().ToTable("Organization", "dbo");
            modelBuilder.Entity<Counter>().ToTable("Counter", "dbo");
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster", "dbo");
            modelBuilder.Entity<UserMaster>().ToTable("UserMaster", "dbo");









        }

    }
}
