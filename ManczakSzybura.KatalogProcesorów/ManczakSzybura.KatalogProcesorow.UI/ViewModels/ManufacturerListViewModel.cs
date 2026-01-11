using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    public class ManufacturerListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ManufacturerViewModel> Manufacturers { get; set; } = new ObservableCollection<ManufacturerViewModel>();

        public void RefreshList(IEnumerable<IManufacturer> manufacturers)
        {
            Manufacturers.Clear();

            foreach (var item in manufacturers)
            {
                Manufacturers.Add(new ManufacturerViewModel(item));
            }

            RaisePropertyChanged(nameof(Manufacturers));
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
