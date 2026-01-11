namespace ManczakSzybura.KatalogProcesorow.Interfaces
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        IManufacturer manufacturer { get; set; }
    }
}
