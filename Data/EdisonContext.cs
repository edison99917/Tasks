using Microsoft.EntityFrameworkCore;

namespace Edison.Data
{
    public class EdisonContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Models.Task> Task { get; set; } = null!;
        public Microsoft.EntityFrameworkCore.DbSet<Models.Asset> Asset { get; set; } = null!;
        public Microsoft.EntityFrameworkCore.DbSet<Models.AssetCategory> AssetCategory { get; set; } = null!;
        public Microsoft.EntityFrameworkCore.DbSet<Models.Department> Department { get; set; } = null!;

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Persist Security Info=True;Integrated Security=SSPI;TrustServerCertificate=True;initial catalog=Edison;");
        }
    }
}
