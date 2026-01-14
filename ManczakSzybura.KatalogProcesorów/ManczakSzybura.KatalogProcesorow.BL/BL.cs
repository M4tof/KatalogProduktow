using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ManczakSzybura.KatalogProcesorow.BL
{
    public class BL
    {
        private IDAO _dao;
        private static BL instance;
        private static readonly object lockObject = new object();
        public BL(IConfiguration configuration)
        {
            // Pobieramy nazwę biblioteki z pliku konfiguracyjnego (wymóg 2.5)
            // Szuka w appsettings.json (IConfiguration) lub App.config (ConfigurationManager)
            string libraryName = configuration["DAOLibraryName"]
                                 ?? System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"]!;

            if (string.IsNullOrEmpty(libraryName))
                throw new Exception("Brak klucza 'DAOLibraryName' w pliku konfiguracyjnym.");

            LoadLibrary(libraryName, configuration);
        }

        // METODA DO LATE BINDINGU (Wymóg 2.4)
        private void LoadLibrary(string libraryName, IConfiguration configuration)
        {
            try
            {
                // 1. Próbujemy znaleźć pełną ścieżkę do pliku DLL
                string? dllPath = FindDllPath(libraryName);

                if (dllPath == null)
                    throw new FileNotFoundException($"Nie można odnaleźć pliku biblioteki: {libraryName} w żadnej znanej lokalizacji.");

                // 2. Ładowanie zestawu (assembly) z pełnej ścieżki
                Assembly assembly = Assembly.UnsafeLoadFrom(dllPath);

                // Szukanie typu implementującego IDAO
                Type? daoType = assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IDAO).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                if (daoType == null)
                    throw new Exception($"W bibliotece {dllPath} nie znaleziono klasy implementującej IDAO.");

                ConstructorInfo? constructor = daoType.GetConstructor(new[] { typeof(IConfiguration) });

                if (constructor != null)
                    _dao = (IDAO)constructor.Invoke(new object[] { configuration });
                else
                    _dao = (IDAO)Activator.CreateInstance(daoType)!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Błąd Late Binding (DAO) dla {libraryName}: {ex.Message}");
            }
        }

        private string? FindDllPath(string libraryName)
        {
            // 1. Sprawdź folder roboczy (tam gdzie jest .exe)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string localPath = Path.Combine(baseDir, libraryName);
            if (File.Exists(localPath)) return localPath;

            // 2. Jeśli nie ma, szukaj w folderach nadrzędnych (do 4 poziomów w górę)
            DirectoryInfo? currentDir = new DirectoryInfo(baseDir);

            for (int i = 0; i < 4; i++)
            {
                if (currentDir == null) break;

                // Szukaj pliku rekurencyjnie w aktualnym folderze (np. w całym Solution)
                // Ograniczamy się do folderów "bin", żeby nie skanować wszystkiego
                var files = currentDir.GetFiles(libraryName, SearchOption.AllDirectories);
                var bestMatch = files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

                if (bestMatch != null) return bestMatch.FullName;

                currentDir = currentDir.Parent;
            }

            return null;
        }

        // GET INSTANCE (zgodny z konstruktorem)
        public static BL GetInstance(IConfiguration configuration)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new BL(configuration);
                    }
                }
            }
            return instance;
        }

        // --- Metody operacyjne (używają tylko interfejsu _dao) ---

        public ICPU NewCpu() => _dao.NewCPU();
        public IEnumerable<ICPU> GetAllCPUs() => _dao.GetAllCPUs();
        public IEnumerable<IManufacturer> GetAllManufacturers() => _dao.GetAllManufaturers();

        public IManufacturer GetManufacturerById(int manId) =>
            _dao.GetAllManufaturers().First(m => m.Id.Equals(manId));

        public ICPU GetCPUById(int cpuId) =>
            _dao.GetAllCPUs().First(m => m.Id.Equals(cpuId));

        public IEnumerable<string> GetAllCPUsNames() => _dao.GetAllCPUs().Select(c => c.Name);
        public IEnumerable<string> GetAllManufacturersNames() => _dao.GetAllManufaturers().Select(m => m.Name);

        public void CreateNewCPU(ICPU cpu) => _dao.CreateCPU(cpu);
        public void CreateCPU(ICPU cpu) => _dao.CreateNewCPU(cpu);
        public void CreateManufacturer(IManufacturer manufacturer) => _dao.CreateNewManufacturer(manufacturer);

        public void DeleteManufacturer(int manufacturerId) => _dao.DeleteManufacturer(manufacturerId);
        public void DeleteCPU(int cpuId) => _dao.DeleteCPU(cpuId);

        public void UpdateManufacturer(IManufacturer manufacturer) => _dao.UpdateManufacturer(manufacturer);
        public void UpdateCPU(ICPU cpu) => _dao.UpdateCPU(cpu);

        public IEnumerable<ICPU> FilterProductByProducer(string producerName) =>
            _dao.GetAllCPUs().Where(c => c.manufacturer.Name.Equals(producerName));

        public IEnumerable<ICPU> FilterProductBySocketType(CPUSocketType socketType) =>
            _dao.GetAllCPUs().Where(c => c.SocketType.Equals(socketType));

        public IEnumerable<ICPU> FilterProductByCores(int cores) =>
            _dao.GetAllCPUs().Where(c => c.Cores.Equals(cores));

        public IEnumerable<IManufacturer> FilterProducerByAddress(string address) =>
            _dao.GetAllManufaturers().Where(p => p.Address.Equals(address));

        public IEnumerable<ICPU> SearchCPUByName(string name) =>
            _dao.GetAllCPUs().Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<IManufacturer> SearchProducerByName(string name) =>
            _dao.GetAllManufaturers().Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<string> GetUniqueAddresses() =>
            _dao.GetAllManufaturers().Select(p => p.Address).Distinct();

        public IEnumerable<string> GetUniqueCores() =>
            _dao.GetAllCPUs().Select(c => c.Cores.ToString()).Distinct();

    }
}