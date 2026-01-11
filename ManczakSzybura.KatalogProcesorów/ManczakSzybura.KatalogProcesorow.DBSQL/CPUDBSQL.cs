using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.DBSQL
{
    public class CPUDBSQL
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int manufacturerId { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public CPUSocketType SocketType { get; set; }


        public ICPU ToICPU(List<ManufacturerDBSQL> manufacturers)
        {
            return new CPU()
            {
                Id = Id,
                Name = Name,
                manufacturer = manufacturers.Single(m => m.Id.Equals(manufacturerId)).ToIManufacturer(),
                Cores = Cores,
                Threads = Threads,
                BaseClockGHz = BaseClockGHz,
                SocketType = SocketType
            };
        }

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
}
