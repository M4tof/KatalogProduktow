using ManczakSzybrua.KatalogProcesorow.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManczakSzybrua.KatalogProcesorow.INTERFACES
{
    public interface ICpu : IProduct
    {
        int Cores { get; set; }
        int Threads { get; set; }
        double BaseClockGHz { get; set; }
        SocketType Socket { get; set; }
    }

}
