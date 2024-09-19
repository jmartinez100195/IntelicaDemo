using Intelica.Authentication.API.Domain.PageAggegate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Intelica.Authentication.API.Common.EFCore.Configuration
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable(nameof(Page)).HasKey(x => x.PageID);
        }
    }
}