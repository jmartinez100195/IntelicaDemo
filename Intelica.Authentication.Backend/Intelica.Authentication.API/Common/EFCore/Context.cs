using Intelica.Authentication.API.Common.EFCore.Configuration;
using Intelica.Authentication.API.Domain.BusinessUserAggregate.Domain;
using Microsoft.EntityFrameworkCore;
namespace Intelica.Security.Domain.Common.EFCore
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
	{
		public DbSet<BusinessUser> BusinessUsers { get; set; }
        public DbSet<BusinessUserPage> BusinessUserPages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new BusinessUserConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessUserPageConfiguration());
        }
	}
}