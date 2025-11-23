using ManczakSzybura.KatalogProduktów.CORE;

namespace MańczakSzybura.KatalogProduktów.INTERFACES;

public interface ICpu : IProduct
{
    int Cores { get; set; }
    int Threads { get; set; }
    double BaseClockGHz { get; set; }
    SocketType Socket { get; set; }
}