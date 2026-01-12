using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.Web.Models
{
    public class CPU : ICPU
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IManufacturer manufacturer { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public CPUSocketType SocketType { get; set; }
    }
}
