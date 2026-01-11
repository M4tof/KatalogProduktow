using ManczakSzybura.KatalogProcesorow.CORE;

namespace ManczakSzybura.KatalogProcesorow.Interfaces
{
    public interface ICPU : IProduct
    {
        int Cores { get; set; }
        int Threads { get; set; }
        double BaseClockGHz { get; set; }
        CPUSocketType SocketType { get; set; }
    }
}
