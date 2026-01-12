using System.ComponentModel.DataAnnotations;

namespace ManczakSzybura.KatalogProcesorow.Web.Models
{
    public class ManufacturerEdit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}