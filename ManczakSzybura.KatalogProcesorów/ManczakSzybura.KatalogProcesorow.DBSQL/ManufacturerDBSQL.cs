using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.DBSQL
{
    public class ManufacturerDBSQL
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public IManufacturer ToIManufacturer()
        {
            return new Manufacturer() { Id = Id, Name = Name, Address = Address };
        }

        public class Manufacturer : IManufacturer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }

        }
    }
}
