using Intelica.Authentication.API.Common.EFCore.Configuration;
using Intelica.Authentication.API.Domain.BusinessUserAggregate.Domain;
using Intelica.Authentication.API.Domain.ClientAggregate.Domain;
using Intelica.Authentication.API.Domain.PageAggegate.Domain;
using Microsoft.EntityFrameworkCore;
namespace Intelica.Security.Domain.Common.EFCore
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
	{
		public DbSet<BusinessUser> BusinessUsers { get; set; }
        public DbSet<BusinessUserPage> BusinessUserPages { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Page> Pages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessUserConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessUserPageConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
        }
	}
}