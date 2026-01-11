using ManczakSzybura.KatalogProcesorow.CORE;
using ManczakSzybura.KatalogProcesorow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ManczakSzybura.KatalogProcesorow.UI
{
    public partial class NewCPU : Window
    {
        // Constructor for ADDING a new CPU
        public NewCPU(IEnumerable<string> manufacturers)
        {
            InitializeComponent();
            PopulateDropDowns(manufacturers);
        }

        // Constructor for EDITING an existing CPU
        public NewCPU(IEnumerable<string> manufacturers, ICPU cpu)
        {
            InitializeComponent();
            PopulateDropDowns(manufacturers);

            // Set current values
            cpuName.Text = cpu.Name;
            manufacturer.SelectedItem = cpu.manufacturer.Name;
            socketType.SelectedItem = cpu.SocketType.ToString();
            cpuCores.Text = cpu.Cores.ToString();
            cpuThreads.Text = cpu.Threads.ToString();
            cpuClock.Text = cpu.BaseClockGHz.ToString();

            headerText.Text = "Edit CPU Information:";
        }

        private void PopulateDropDowns(IEnumerable<string> manufacturers)
        {
            // Fill Manufacturers
            manufacturer.ItemsSource = manufacturers.ToList();
            if (manufacturer.Items.Count > 0) manufacturer.SelectedIndex = 0;

            // Fill Socket Types from Enum
            socketType.ItemsSource = Enum.GetNames(typeof(CPUSocketType));
            if (socketType.Items.Count > 0) socketType.SelectedIndex = 0;
        }

        #region Properties for Main Window to Read
        public string CPUName => cpuName.Text;

        public CPUSocketType CPUSocket
        {
            get
            {
                Enum.TryParse(socketType.Text, out CPUSocketType type);
                return type;
            }
        }

        public string SelectedManufacturer => manufacturer.Text;

        public int Cores => int.TryParse(cpuCores.Text, out int val) ? val : 0;

        public int Threads => int.TryParse(cpuThreads.Text, out int val) ? val : 0;

        public double BaseClock => double.TryParse(cpuClock.Text, out double val) ? val : 0.0;
        #endregion

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(cpuName.Text))
            {
                MessageBox.Show("Please enter a CPU name.");
                return;
            }

            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            cpuName.SelectAll();
            cpuName.Focus();
        }

        // Logic for Integer only fields
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Logic for Decimal fields (allows one dot or comma depending on culture)
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}