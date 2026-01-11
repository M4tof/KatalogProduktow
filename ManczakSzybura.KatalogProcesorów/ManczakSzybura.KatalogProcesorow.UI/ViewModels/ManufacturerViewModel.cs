using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.ComponentModel;

namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    public class ManufacturerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IManufacturer manufacturer;

        public ManufacturerViewModel(IManufacturer manufacturer)
        {
            this.manufacturer = manufacturer;
        }

        public int ManufacturerId
        {
            get => manufacturer.Id;
            set
            {
                manufacturer.Id = value;
                RaisePropertyChanged(nameof(ManufacturerId));
            }
        }

        public string ManufacturerName
        {
            get => manufacturer.Name;
            set
            {
                manufacturer.Name = value;
                RaisePropertyChanged(nameof(ManufacturerName));
            }
        }

        public string ManufacturerAddress
        {
            get => manufacturer.Address;
            set
            {
                manufacturer.Address = value;
                RaisePropertyChanged(nameof(ManufacturerAddress));
            }
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
