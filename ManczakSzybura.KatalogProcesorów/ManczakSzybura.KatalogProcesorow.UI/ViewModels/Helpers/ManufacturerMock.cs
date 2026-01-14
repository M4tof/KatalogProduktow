using ManczakSzybura.KatalogProcesorow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    public class Manufacturer : IManufacturer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Address { get; set; }
    }
}
