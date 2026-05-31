using Microsoft.EntityFrameworkCore;
using projekt_Jakub_Siwek_Z403_AV.Models;

namespace projekt_Jakub_Siwek_Z403_AV.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=JiPP;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }
    }
}