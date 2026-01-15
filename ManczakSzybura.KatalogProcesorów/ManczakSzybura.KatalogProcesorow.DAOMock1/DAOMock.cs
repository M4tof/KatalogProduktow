
using ManczakSzybrua.KatalogProcesorow.DAOMock1.BO;
using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.DAOMock1.BO;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System;

namespace ManczakSzybura.KatalogProcesorow.DAOMock1
{
    public class DAOMock : IDAO
    {
        private List<IManufacturer> manufacturers;
        private List<ICPU> cPUs;
        private int nextIdCpu = 2;
        private int nextIdMan = 2;

        /// <summary>
        /// Podstawowy konstruktor
        /// </summary>
        public DAOMock()
        {
            manufacturers = new List<IManufacturer>()
            {
                   new Manufacturer()
                {
                    Id = 1,
                    Name = "Intel Corporation",
                    Address = "2200 Mission College Blvd, Santa Clara, CA 95054"
                }
            };


            cPUs = new List<ICPU>()
            {
                new CPU()
                {
                    Id = 1,
                    Name = "Intel Core i9-14900K",
                    manufacturer = manufacturers[0], // Intel
                    Cores = 24,
                    Threads = 32,
                    BaseClockGHz = 3.2,
                    SocketType = CPUSocketType.Socket1700
                }
            };
        }

        //CREATE
        public void CreateCPU(ICPU cpu)
        {
            CreateNewCPU(cpu);
        }

        public ICPU CreateNewCPU(ICPU cpu)
        {
            cpu.Id = nextIdCpu++;
            Console.WriteLine("new cpu in mock1");
            cPUs.Add(cpu);
            return cpu;
        }

        public IManufacturer CreateNewManufacturer(IManufacturer manufacturer)
        {
            manufacturer.Id = nextIdMan++;
            Console.WriteLine("new manufacturer in mock1");
            manufacturers.Add(manufacturer);
            return manufacturer;
        }

        //DELETE
        public void DeleteCPU(int cpuId)
        {
            ICPU cpuDelete = cPUs.First(p => p.Id.Equals(cpuId));
            cPUs.Remove(cpuDelete);
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            IManufacturer manDelete = manufacturers.First(p => p.Id.Equals(manufacturerId));
            manufacturers.Remove(manDelete);
        }

        //GETERS
        public IEnumerable<ICPU> GetAllCPUs()
        {
            return cPUs;
        }

        public IEnumerable<IManufacturer> GetAllManufaturers()
        {
            return manufacturers;
        }


        //UPDATE
        public void UpdateCPU(ICPU cpu)
        {
            int cpuID = cPUs.FindIndex(Old => Old.Id.Equals(cpu.Id));
            if (cpuID != -1)
            {
                cPUs[cpuID] = cpu;
            }
            else
            {
                CreateNewCPU(cpu);
            }
        }

        public void UpdateManufacturer(IManufacturer manufacturer)
        {
            int manId = manufacturers.FindIndex(manOld => manOld.Id.Equals(manufacturer.Id));
            if (manId != -1)
            {
                manufacturers[manId] = manufacturer;
            }
            else
            {
                CreateNewManufacturer(manufacturer);
            }
        }
    }
}
