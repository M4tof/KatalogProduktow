using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program booted: Hello World!");
            string libraryName = System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"];
            BL.BL bl = new BL.BL(libraryName);

            Console.WriteLine("Producers--------------------");
            foreach (IManufacturer m in bl.GetAllManufacturers())
            {
                Console.WriteLine($"manufacturer: {m.Id}, {m.Name}, {m.Address}");
            }

            Console.WriteLine("Products--------------------");
            foreach(ICPU c in bl.GetAllCPUs())
            {
                Console.WriteLine($"cpu: {c.Id}, {c.Name}, {c.SocketType}, {c.Cores}, {c.manufacturer}");
            }
        }
    }
}