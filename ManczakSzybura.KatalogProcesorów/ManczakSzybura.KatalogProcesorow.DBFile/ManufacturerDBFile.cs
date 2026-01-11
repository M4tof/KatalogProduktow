using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.Runtime.Serialization;

namespace ManczakSzybura.KatalogProcesorow.DBFile
{
    public class ManufacturerDBFile : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

}
