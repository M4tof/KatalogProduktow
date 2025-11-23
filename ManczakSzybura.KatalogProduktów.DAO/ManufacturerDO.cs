using MańczakSzybura.KatalogProduktów.INTERFACES;

namespace ManczakSzybura.KatalogProduktów.DAO;

public class ManufacturerDo : IManufacturer
{
    public int Id { get; set; }
    public string Name { get; set; }
}