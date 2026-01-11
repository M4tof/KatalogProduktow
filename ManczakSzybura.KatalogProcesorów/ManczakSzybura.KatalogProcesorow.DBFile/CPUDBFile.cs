using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ManczakSzybura.KatalogProcesorow.DBFile
{
    public class CPUDBFile : ICPU
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public CPUSocketType SocketType { get; set; }
        public int ManufacturerId { get; set; }

        [JsonIgnore]
        public IManufacturer manufacturer { get; set; }
    }

}
