using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybura.KatalogProcesorow.DAOMock1.BO
{
    [Serializable]
    public class CPU : ICPU
    {
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public CPUSocketType SocketType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IManufacturer manufacturer { get; set; }
    }
}
