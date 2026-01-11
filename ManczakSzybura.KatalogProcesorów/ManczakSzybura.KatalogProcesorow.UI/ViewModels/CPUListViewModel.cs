using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    public class CPUListViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CPUViewModel> CPUs { get; set; } = new ObservableCollection<CPUViewModel>();

        public void RefreshList(IEnumerable<ICPU> cPUs)
        {
            CPUs.Clear();

            foreach(var item in cPUs)
            {
                CPUs.Add(new CPUViewModel(item));
            }

            RaisePropertyChanged(nameof(CPUs));
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
