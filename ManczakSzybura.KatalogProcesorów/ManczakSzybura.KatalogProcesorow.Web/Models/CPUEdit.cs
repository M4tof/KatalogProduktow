using ManczakSzybura.KatalogProcesorow.CORE;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.Web.Models
{
    public class CPUEdit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Range(1, 256)]
        public int Cores { get; set; }

        [Range(1, 512)]
        public int Threads { get; set; }

        [Display(Name = "Base Clock (GHz)")]
        public double BaseClockGHz { get; set; }

        [Display(Name = "Socket Type")]
        public CPUSocketType SocketType { get; set; }

        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        public List<SelectListItem> Manufacturers { get; set; } = new List<SelectListItem>();
    }
}