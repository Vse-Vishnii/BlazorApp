using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorApp.Data
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole {Name = "visitor", NormalizedName = "VISITOR"},
                new IdentityRole {Name = "admin", NormalizedName = "ADMIN"});
        }
    }
}
