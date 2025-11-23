using ManczakSzybrua.KatalogProcesorow.INTERFACES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManczakSzybrua.KatalogProcesorow.DAO
{
    public class ManufacturerDO : IManufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
