using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System.ComponentModel;


namespace ManczakSzybura.KatalogProcesorow.UI.ViewModels
{
    public class CPUViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ICPU CPU;

        public CPUViewModel(ICPU CPU)
        {
            this.CPU = CPU;
        }

        public int CPUID
        {
            get => CPU.Id;
            set
            {
                CPU.Id = value;
                RaisePropertyChanged(nameof(CPUID));
            }
        }

        public string CPUName
        {
            get => CPU.Name;
            set
            {
                CPU.Name = value;
                RaisePropertyChanged(nameof(CPUName));
            }
        }

        public string CPUProducerName
        {
            get => CPU.manufacturer.Name;
            set
            {
                CPU.manufacturer.Name = value;
                RaisePropertyChanged(nameof(CPUProducerName));
            }
        }

        public string CPUProducerAddress
        {
            get => CPU.manufacturer.Address;
            set
            {
                CPU.manufacturer.Address = value;
                RaisePropertyChanged(nameof(CPUProducerAddress));
            }
        }

        public int CPUCores
        {
            get => CPU.Cores;
            set
            {
                CPU.Cores = value;
                RaisePropertyChanged(nameof(CPUCores));
            }
        }

        public double CPUBaseClockGhz
        {
            get => CPU.BaseClockGHz;
            set
            {
                CPU.BaseClockGHz = value;
                RaisePropertyChanged(nameof(CPUBaseClockGhz));
            }
        }

        public int CPUThreads
        {
            get => CPU.Threads;
            set
            {
                CPU.Threads = value;
                RaisePropertyChanged(nameof(CPUThreads));
            }
        }

        public CPUSocketType CPUSocketType
        {
            get => CPU.SocketType;
            set
            {
                CPU.SocketType = value;
                RaisePropertyChanged(nameof(CPUSocketType));
            }
        }
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
