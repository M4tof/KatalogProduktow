using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    // Klasy pomocnicze implementujące interfejsy wewnątrz UI (zgodnie z brakiem referencji do DAO)
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
