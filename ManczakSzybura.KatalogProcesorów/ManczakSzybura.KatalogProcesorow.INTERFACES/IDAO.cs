namespace ManczakSzybura.KatalogProcesorow.Interfaces
{
    public interface IDAO
    {
        //GETERS
        IEnumerable<IManufacturer> GetAllManufaturers();
        //IEnumerable<IProduct> GetAllProducts();
        IEnumerable<ICPU> GetAllCPUs();

        //CREATES
        IManufacturer CreateNewManufacturer(IManufacturer manufacturer);
        //IProduct CreateNewProduct(IProduct product);
        ICPU CreateNewCPU(ICPU cpu);

        //DELETES
        void DeleteManufacturer(int manufacturerId);
        //void DeleteProduct(int productId);
        void DeleteCPU(int cpuId);

        //UPDATES
        void UpdateManufacturer(IManufacturer manufacturer);
        void UpdateCPU(ICPU cpu);
    }
}
