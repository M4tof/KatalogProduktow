using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManczakSzybrua.KatalogProcesorow.INTERFACES
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        int ManufacturerId { get; set; }
    }
}
