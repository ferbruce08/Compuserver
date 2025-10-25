using Microsoft.EntityFrameworkCore;
using CompuserverBackend.Models;

namespace CompuserverBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Este constructor vacío es útil para migraciones
        public ApplicationDbContext() { }

        public DbSet<UserAction> UserActions { get; set; }

        // ⚡ Agrega este método para que EF sepa cómo conectarse al hacer migraciones
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Ajusta la cadena de conexión según tu SQL Server
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=CompuserverDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
    }
}




