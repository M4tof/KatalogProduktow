using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.Web.Models
{
    public class ManufacturerCreate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}