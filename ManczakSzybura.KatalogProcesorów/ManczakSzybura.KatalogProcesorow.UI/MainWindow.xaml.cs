using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using ManczakSzybura.KatalogProcesorow.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static ManczakSzybura.KatalogProcesorow.DBSQL.ManufacturerDBSQL;

namespace ManczakSzybura.KatalogProcesorow.UI
{
    public partial class MainWindow : Window
    {
        public ViewModels.CPUListViewModel CPULVM { get; } = new ViewModels.CPUListViewModel();
        private ViewModels.CPUViewModel selectedCPU = null;

        public ViewModels.ManufacturerListViewModel ManufacturerLVM { get; } = new ViewModels.ManufacturerListViewModel();
        private ViewModels.ManufacturerViewModel selectedManufacturer = null;

        private readonly BL.BL bl;
        private string selectedDAO = System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"]!;

        public MainWindow()
        {
            bl = new BL.BL(selectedDAO);
            InitializeComponent();
            RefreshAll();
        }

        private void RefreshAll()
        {
            // Refresh ViewModels from Business Logic
            ManufacturerLVM.RefreshList(bl.GetAllManufacturers().Distinct());
            CPULVM.RefreshList(bl.GetAllCPUs());

            // Update filter dropdowns
            manufacturerFilterValueComboBox.ItemsSource = GetAddresses();

            // If the CPU filter is currently set to manufacturers, refresh that list too
            if (filterTypeComboBox.SelectedItem is ComboBoxItem cbi && cbi.Content.ToString() == "manufacturer")
            {
                filterValueComboBox.ItemsSource = bl.GetAllManufacturers().Select(m => m.Name).Distinct();
            }
        }

        private IEnumerable<string> GetAddresses()
        {
            return bl.GetAllManufacturers().Select(m => m.Address).Distinct();
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
                    filterValueComboBox.ItemsSource = bl.GetAllManufacturers().Select(m => m.Name).Distinct();
                    break;
                case "number of cores":
                    filterValueComboBox.ItemsSource = bl.GetAllCPUs().Select(c => c.Cores.ToString()).Distinct();
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
                CPULVM.RefreshList(bl.GetAllCPUs());
                return;
            }

            var all = bl.GetAllCPUs();
            switch (selectedType)
            {
                case "socket type":
                    CPULVM.RefreshList(all.Where(c => c.SocketType.ToString() == filterValue));
                    break;
                case "manufacturer":
                    CPULVM.RefreshList(all.Where(c => c.manufacturer.Name == filterValue));
                    break;
                case "number of cores":
                    int.TryParse(filterValue, out int cores);
                    CPULVM.RefreshList(all.Where(c => c.Cores == cores));
                    break;
            }
        }

        private void ApplyCPUSearch(object sender, RoutedEventArgs e)
        {
            string search = cpuSearchField.Text;
            CPULVM.RefreshList(string.IsNullOrWhiteSpace(search)
                ? bl.GetAllCPUs()
                : bl.GetAllCPUs().Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        private void RemoveFiltersCPU(object sender, RoutedEventArgs e)
        {
            cpuSearchField.Clear();
            filterValueComboBox.SelectedItem = null;
            CPULVM.RefreshList(bl.GetAllCPUs());
        }

        private void ManufacturerApplyFilter(object sender, RoutedEventArgs e)
        {
            string address = manufacturerFilterValueComboBox.SelectedItem as string;
            ManufacturerLVM.RefreshList(string.IsNullOrEmpty(address)
                ? bl.GetAllManufacturers()
                : bl.GetAllManufacturers().Where(m => m.Address == address));
        }

        private void ApplyManufacturerSearch(object sender, RoutedEventArgs e)
        {
            string search = manufacturerSearchField.Text;
            ManufacturerLVM.RefreshList(string.IsNullOrWhiteSpace(search)
                ? bl.GetAllManufacturers()
                : bl.GetAllManufacturers().Where(m => m.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        private void RemoveFiltersManufacturer(object sender, RoutedEventArgs e)
        {
            manufacturerSearchField.Clear();
            manufacturerFilterValueComboBox.SelectedItem = null;
            ManufacturerLVM.RefreshList(bl.GetAllManufacturers());
        }
        #endregion

        #region CPU Operations

        private void CPUList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCPU = CPUList.SelectedItem as CPUViewModel;
        }

        private void AddCPU(object sender, RoutedEventArgs e)
        {
            var manufacturers = bl.GetAllManufacturers().Select(m => m.Name);
            NewCPU dialog = new NewCPU(manufacturers);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    int newId = CPULVM.CPUs.Any() ? CPULVM.CPUs.Max(c => c.CPUID) + 1 : 1;
                    var manufacturerObj = bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

                    ICPU newCpu = new CPU()
                    {
                        Id = newId,
                        Name = dialog.CPUName,
                        Cores = dialog.Cores,
                        Threads = dialog.Threads,
                        BaseClockGHz = dialog.BaseClock,
                        SocketType = dialog.CPUSocket,
                        manufacturer = manufacturerObj
                    };

                    // Using the BL method
                    bl.CreateCPU(newCpu);
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
            if (selectedCPU == null)
            {
                MessageBox.Show("Please select a CPU from the list first.");
                return;
            }

            ICPU currentCpu = bl.GetCPUById(selectedCPU.CPUID);
            var manufacturers = bl.GetAllManufacturers().Select(m => m.Name);
            NewCPU dialog = new NewCPU(manufacturers, currentCpu);

            if (dialog.ShowDialog() == true)
            {
                var manufacturerObj = bl.GetAllManufacturers().First(m => m.Name == dialog.SelectedManufacturer);

                currentCpu.Name = dialog.CPUName;
                currentCpu.Cores = dialog.Cores;
                currentCpu.Threads = dialog.Threads;
                currentCpu.BaseClockGHz = dialog.BaseClock;
                currentCpu.SocketType = dialog.CPUSocket;
                currentCpu.manufacturer = manufacturerObj;

                bl.UpdateCPU(currentCpu);
                RefreshAll();
            }
        }


        private void RemoveCPU(object sender, RoutedEventArgs e)
        {
            if (selectedCPU == null)
            {
                MessageBox.Show("Please select a CPU from the list first.");
                return;
            }

            if (MessageBox.Show($"Remove {selectedCPU.CPUName}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bl.DeleteCPU(selectedCPU.CPUID);
                RefreshAll();
                selectedCPU = null; // Reset selection
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
                int newId = ManufacturerLVM.Manufacturers.Any() ? ManufacturerLVM.Manufacturers.Max(m => m.ManufacturerId) + 1 : 1;

                IManufacturer manufacturer = new Manufacturer()
                {
                    Id = newId,
                    Name = dialog.ManufacturerName,
                    Address = dialog.ManufacturerAddress
                };

                bl.CreateManufacturer(manufacturer);
                RefreshAll();
            }
        }

        private void EditManufacturer(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer == null)
            {
                MessageBox.Show("Please select a manufacturer from the list first.");
                return;
            }

            IManufacturer current = bl.GetManufacturerById(selectedManufacturer.ManufacturerId);
            NewManufacturer dialog = new NewManufacturer(current);

            if (dialog.ShowDialog() == true)
            {
                current.Name = dialog.ManufacturerName;
                current.Address = dialog.ManufacturerAddress;

                bl.UpdateManufacturer(current);
                RefreshAll();
            }
        }


        private void RemoveManufacturer(object sender, RoutedEventArgs e)
        {
            if (selectedManufacturer == null) return;

            if (MessageBox.Show("Removing a manufacturer may affect associated CPUs. Continue?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bl.DeleteManufacturer(selectedManufacturer.ManufacturerId);
                RefreshAll();
            }
        }
        #endregion
    }

    public class CPU : ICPU
    {
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public CPUSocketType SocketType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IManufacturer manufacturer { get; set; }
    }
}