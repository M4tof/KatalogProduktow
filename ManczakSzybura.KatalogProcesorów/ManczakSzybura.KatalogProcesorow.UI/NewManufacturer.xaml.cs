using ManczakSzybura.KatalogProcesorow.Interfaces;
using System;
using System.Windows;

namespace ManczakSzybura.KatalogProcesorow.UI
{
    public partial class NewManufacturer : Window
    {
        // Constructor for ADDING
        public NewManufacturer()
        {
            InitializeComponent();
            headerText.Text = "Add New Manufacturer";
        }

        // Constructor for EDITING
        public NewManufacturer(IManufacturer manufacturer)
        {
            InitializeComponent();
            headerText.Text = "Edit Manufacturer";
            manufacturerName.Text = manufacturer.Name;
            manufacturerAddress.Text = manufacturer.Address;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(manufacturerName.Text))
            {
                MessageBox.Show("Please provide a manufacturer name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            manufacturerName.SelectAll();
            manufacturerName.Focus();
        }

        public string ManufacturerName
        {
            get { return manufacturerName.Text; }
        }

        public string ManufacturerAddress
        {
            get { return manufacturerAddress.Text; }
        }
    }
}