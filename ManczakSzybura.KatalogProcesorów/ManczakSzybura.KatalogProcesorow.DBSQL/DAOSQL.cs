using ManczakSzybura.KatalogProcesorow.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ManczakSzybura.KatalogProcesorow.DBSQL
{
    public class DAOSQL : DbContext, IDAO
    {
        public DbSet<CPUDBSQL> cPUs { get; set; }
        public DbSet<ManufacturerDBSQL> Manufacturers { get; set; }
        public string DbPath { get; }

        private IConfiguration _configuration;

        /// <summary>
        /// Konstruktor z konfiguracji
        /// </summary>
        /// <param name="configuration">gotowa konfiguracja</param>
        public DAOSQL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Podstawowy konstruktor
        /// </summary>
        public DAOSQL()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join("", "cpucatalog.db");
        }

        /// <summary>
        /// Konstruktor z niestandardową ścieżką
        /// </summary>
        /// <param name="dbFilePath">scieżka do pliku bazodanowego</param>
        public DAOSQL(string dbFilePath)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(dbFilePath);
        }

        /// <summary>
        /// Konfiguracja bazy danych
        /// </summary>
        /// <param name="options">opcje budowy</param>
        /// <exception cref="InvalidOperationException">Exception zwracane przez biblioteke</exception>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string projectRootDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;

            if (projectRootDirectory != null)
            {

                string dbFilePath = Path.Combine(projectRootDirectory, "cpucatalog.db");
                options.UseSqlite($"Data Source={dbFilePath}");
            }
            else
            {
                throw new InvalidOperationException("Failed to determine the project's root directory.");
            }
        }

        //GETERS
        public IEnumerable<IManufacturer> GetAllManufaturers() => Manufacturers.Select(m => m.ToIManufacturer());

        public IEnumerable<ICPU> GetAllCPUs()
        {
            return cPUs.Select(c => c.ToICPU(Manufacturers.ToList()));
        }

        //CREATES
        public IManufacturer CreateNewManufacturer(IManufacturer manufacturer)
        {
            Add(new ManufacturerDBSQL() { Id = manufacturer.Id, Name = manufacturer.Name, Address = manufacturer.Address });
            SaveChanges();
            return manufacturer;
        }

        public ICPU CreateNewCPU(ICPU cpu)
        {
            Add(new CPUDBSQL()
            {
                Id = cpu.Id,
                Name = cpu.Name,
                BaseClockGHz = cpu.BaseClockGHz,
                Cores = cpu.Cores,
                Threads = cpu.Threads,
                SocketType = cpu.SocketType,
                manufacturerId = cpu.manufacturer.Id
            });
            SaveChanges();
            return cpu;
        }

        //DELETES
        public void DeleteManufacturer(int manufacturerId)
        {
            var manufacturerToDelete = Manufacturers.FirstOrDefault(m  => m.Id == manufacturerId);
            Remove(manufacturerToDelete);
            SaveChanges();
        }

        public void DeleteCPU(int cpuId)
        {
            var cpuToDelete = cPUs.FirstOrDefault(c => c.Id == cpuId);
            Remove(cpuToDelete);
            SaveChanges();
        }

        //UPDATES
        public void UpdateManufacturer(IManufacturer manufacturer)
        {
            var newManufacturer = Manufacturers.FirstOrDefault(m => m.Id.Equals(manufacturer.Id));
            
            //Zamień wartości, zostaw indeks
            newManufacturer.Name = manufacturer.Name;
            newManufacturer.Address = manufacturer.Address;

            Entry(newManufacturer).CurrentValues.SetValues(newManufacturer);
            SaveChanges();
        }
        public void UpdateCPU(ICPU cpu)
        {
            var newCPU = cPUs.FirstOrDefault(c => c.Id.Equals(cpu.Id));

            newCPU.Name = cpu.Name;
            newCPU.Cores = cpu.Cores;
            newCPU.Threads = cpu.Threads;
            newCPU.BaseClockGHz = cpu.BaseClockGHz;
            newCPU.SocketType = cpu.SocketType;
            newCPU.manufacturerId = cpu.manufacturer.Id;

            Entry(newCPU).CurrentValues.SetValues(newCPU);
            SaveChanges();
        }

    }
}
