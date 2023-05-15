using Microsoft.EntityFrameworkCore;

namespace Tasks.Data
{
    public class EdisonContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Models.Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Persist Security Info=True;Integrated Security=SSPI;TrustServerCertificate=True;initial catalog=Edison;");
        }
    }
}
