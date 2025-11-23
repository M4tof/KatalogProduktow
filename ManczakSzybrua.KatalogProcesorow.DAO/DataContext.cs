using ManczakSzybrua.KatalogProcesorow.DAO;
using Microsoft.EntityFrameworkCore;

namespace ManczakSzybura.KatalogProduktów.DAO
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source = models.db");
        }

        public virtual DbSet<ManufacturerDo> Manufacturers { get; set; }
        public virtual DbSet<CPU_DO> Cpus { get; set; }


    }
}
