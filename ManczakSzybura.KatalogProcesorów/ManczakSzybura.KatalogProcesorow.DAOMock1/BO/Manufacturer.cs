
using ManczakSzybura.KatalogProcesorow.Interfaces;

namespace ManczakSzybrua.KatalogProcesorow.DAOMock1.BO
{
    internal class Manufacturer : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
