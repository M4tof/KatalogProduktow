namespace MańczakSzybura.KatalogProduktów.INTERFACES;

public interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    int ManufacturerId { get; set; }
}