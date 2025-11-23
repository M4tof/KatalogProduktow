using MańczakSzybura.KatalogProduktów.INTERFACES;
using SocketType = ManczakSzybura.KatalogProduktów.CORE.SocketType;

namespace ManczakSzybura.KatalogProduktów.DAO;

public class CPU_DO : ICpu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ManufacturerId { get; set; }
    public int Cores { get; set; }
    public int Threads { get; set; }
    public double BaseClockGHz { get; set; }
    public SocketType Socket { get; set; }
}