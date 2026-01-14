using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using ManczakSzybura.KatalogProcesorow.UI.ViewModels;
using Microsoft.Extensions.Configuration; // Dodaj to
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ManczakSzybura.KatalogProcesorow.UI
{
    public partial class MainWindow : Window
    {
        public ViewModels.CPUListViewModel CPULVM { get; } = new ViewModels.CPUListViewModel();
        private ViewModels.CPUViewModel selectedCPU = null;

        public ViewModels.ManufacturerListViewModel ManufacturerLVM { get; } = new ViewModels.ManufacturerListViewModel();
        private ViewModels.ManufacturerViewModel selectedManufacturer = null;

        private readonly BL.BL _bl;

        public MainWindow()
        {
            // SZYBKI FIX: Tworzymy pusty obiekt konfiguracji. 
            // Twoja klasa BL sama wyciągnie "DAOLibraryName" z App.config przez ConfigurationManager.
            IConfiguration config = new ConfigurationBuilder().Build();

            _bl = BL.BL.GetInstance(config);

            InitializeComponent();
            RefreshAll();
        }

        private void RefreshAll()
        {
            // Refresh ViewModels from Business Logic
            ManufacturerLVM.RefreshList(_bl.GetAllManufacturers());
            CPULVM.RefreshList(_bl.GetAllCPUs());

            // Update filter dropdowns
            manufacturerFilterValueComboBox.ItemsSource = GetAddresses();

            if (filterTypeComboBox.SelectedItem is ComboBoxItem cbi && cbi.Content.ToString() == "manufacturer")
            {
                filterValueComboBox.ItemsSource = _bl.GetAllManufacturers().Select(m => m.Name).Distinct();
            }
        }

        private IEnumerable<string> GetAddresses()
        {
            return _bl.GetUniqueAddresses(); // Używamy gotowej metody z BL
        }

        #region Filters
        private void FilterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filterValueComboBox == null) return;
            string selected = (filterTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selected)
            {
                case "socket type":
                    filterValueComboBox.ItemsSource = Enum.GetNames(typeof(CPUSocketType));
                    break;
                case "manufacturer":
                    filterValueComboBox.ItemsSource = _bl.GetAllManufacturersNames();
                    break;
                case "number of cores":
                    filterValueComboBox.ItemsSource = _bl.GetUniqueCores();
                    break;
                default:
                    filterValueComboBox.ItemsSource = null;
                    break;
            }
        }

        private void CPUApplyFilter(object sender, RoutedEventArgs e)
        {
            var selectedType = (filterTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var filterValue = filterValueComboBox.SelectedItem as string;

            if (string.IsNullOrEmpty(filterValue))
            {
                CPULVM.RefreshList(_bl.GetAllCPUs());
                return;
            }

            switch (selectedType)
            {
                case "socket type":
                    Enum.TryParse(filterValue, out CPUSocketType socket);
                    CPULVM.RefreshList(_bl.FilterProductBySocketType(socket));
                    break;
                case "manufacturer":
                    CPULVM.RefreshList(_bl.FilterProductByProducer(filterValue));
                    break;
                case "number of cores":
                    int.TryParse(filterValue, out int cores);
                    CPULVM.RefreshList(_bl.FilterProductByCores(cores));
                    break;
            }
        }

        private void ApplyCPUSearch(object sender, RoutedEventArgs e)
        {
            CPULVM.RefreshList(_bl.SearchCPUByName(cpuSearchField.Text));
        }

        private void RemoveFiltersCPU(object sender, RoutedEventArgs e)
        {
            cpuSearchField.Clear();
            filterValueComboBox.SelectedItem = null;
            CPULVM.RefreshList(_bl.GetAllCPUs());
        }

        private void ManufacturerApplyFilter(object sender, RoutedEventArgs e)
        {
            string address = manufacturerFilterValueComboBox.SelectedItem as string;
            ManufacturerLVM.RefreshList(string.IsNullOrEmpty(address)
                ? _bl.GetAllManufacturers()
                : _bl.FilterProducerByAddress(address));
        }

        private void ApplyManufacturerSearch(object sender, RoutedEventArgs e)
        {
            ManufacturerLVM.RefreshList(_bl.SearchProducerByName(manufacturerSearchField.Text));
        }

        private void RemoveFiltersManufacturer(object sender, RoutedEventArgs e)
        {
            manufacturerSearchField.Clear();
            manufacturerFilterValueComboBox.SelectedItem = null;
            ManufacturerLVM.RefreshList(_bl.GetAllManufacturers());
        }
        #endregion

        #region CPU Operations
        private void CPUList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCPU = CPUList.SelectedItem as CPUViewModel;
        }

        private void AddCPU(object sender, RoutedEventArgs e)
        {
            var manufacturers = _bl.GetAllManufacturersNames();
            NewCPU dialog = new NewCPU(manufacturers);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var manufacturerObj = _bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

                    ICPU newCpu = new CPU()
                    {
                        Name = dialog.CPUName,
                        Cores = dialog.Cores,
                        Threads = dialog.Threads,
                        BaseClockGHz = dialog.BaseClock,
                        SocketType = dialog.CPUSocket,
                        manufacturer = manufacturerObj
                    };

                    _bl.CreateCPU(newCpu);
                    RefreshAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding CPU: " + ex.Message);
                }
            }
        }

        private void EditCPU(object sender, RoutedEventArgs e)
        {
            if (selectedCPU == null) return;

            ICPU currentCpu = _bl.GetCPUById(selectedCPU.CPUID);
            var manufacturers = _bl.GetAllManufacturersNames();
            NewCPU dialog = new NewCPU(manufacturers, currentCpu);

            if (dialog.ShowDialog() == true)
            {
                var manufacturerObj = _bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

                currentCpu.Name = dialog.CPUName;
                currentCpu.Cores = dialog.Cores;
                currentCpu.Threads = dialog.Threads;
                currentCpu.BaseClockGHz = dialog.BaseClock;
                currentCpu.SocketType = dialog.CPUSocket;
                currentCpu.manufacturer = manufacturerObj;

                _bl.UpdateCPU(currentCpu);
                RefreshAll();
            }
        }

        private void RemoveCPU(object sender, RoutedEventArgs e)
        {
            if (selectedCPU == null) return;
            if (MessageBox.Show($"Remove {selectedCPU.CPUName}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _bl.DeleteCPU(selectedCPU.CPUID);
                RefreshAll();
                selectedCPU = null;
            }
        }
        #endregion

        #region Manufacturer Operations
        private void ManufacturerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedManufacturer = ManufacturerList.SelectedItem as ManufacturerViewModel;
        }

        private void AddManufacturer(object sender, RoutedEventArgs e)
        {
            NewManufacturer dialog = new NewManufacturer();
            if (dialog.ShowDialog() == true)
            {
                IManufacturer manufacturer = new Manufacturer()
                {
                    Name = dialog.ManufacturerName,
                    Address = dialog.ManufacturerAddress
                };

                _bl.CreateManufacturer(manufacturer);
                RefreshAll();
            }
        }

        private void EditManufacturer(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer == null) return;

            IManufacturer current = _bl.GetManufacturerById(selectedManufacturer.ManufacturerId);
            NewManufacturer dialog = new NewManufacturer(current);

            if (dialog.ShowDialog() == true)
            {
                current.Name = dialog.ManufacturerName;
                current.Address = dialog.ManufacturerAddress;

                _bl.UpdateManufacturer(current);
                RefreshAll();
            }
        }

        private void RemoveManufacturer(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer == null) return;
            if (MessageBox.Show("Remove manufacturer?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _bl.DeleteManufacturer(selectedManufacturer.ManufacturerId);
                RefreshAll();
                selectedManufacturer = null;
            }
        }
        #endregion
    }
}