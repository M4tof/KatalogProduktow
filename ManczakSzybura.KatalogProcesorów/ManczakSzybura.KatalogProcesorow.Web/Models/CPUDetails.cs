using ManczakSzybura.KatalogProcesorow.CORE;
using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.Web.Models
{
    public class CPUDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cores { get; set; }
        public int Threads { get; set; }

        [Display(Name = "Base Clock (GHz)")]
        public double BaseClockGHz { get; set; }

        [Display(Name = "Socket Type")]
        public CPUSocketType SocketType { get; set; }

        public string ManufacturerName { get; set; }
    }
}