using AdministratorClient.Command;
using AdministratorClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdministratorClient.ViewModel
{
    public class EditProductViewModel : INotifyPropertyChanged
    {
        public EditProductViewModel()
        {
            GetProducts();
            GetManufacturers();

        }
        private const string path = "http://localhost:5125";
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged("Products");
                }
            }
        }

        private ObservableCollection<Manufacturer> manufacturers;
        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                if (manufacturers != value)
                {
                    manufacturers = value;
                    OnPropertyChanged("Manufacturers");
                }
            }
        }
        private Manufacturer manufacturerId;
        public Manufacturer ManufacturerId
        {
            get { return manufacturerId; }
            set
            {

                manufacturerId = value;
                OnPropertyChanged("ManufacturerId");

            }

        }
        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }


        
        private Product selectedManufacturer;
        public Product SelectedManufacturer
        {
            get { return selectedManufacturer; }
            set
            {
                selectedManufacturer = value;
                OnPropertyChanged("SelectedManufacturer");
            }
        }
        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(obj =>
                    {

                        if (obj is Product prod)
                        {
                            EditProduct(prod);
                        }

                    }));
            }
        }
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        int code = 0;
                        if (obj is Product prod)
                        {
                            code = DeleteProduct(prod);
                            if (code == 200)
                                Products.Remove(prod);
                            else
                                MessageBox.Show("Произошла ошибка\n Данные не удалены");
                        }




                    },
                    (obj) => Manufacturers.Count > 0));
            }
        }

        private void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.GetAsync($"{path}/product").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null)
                    Products = resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;
            }
        }
        private void GetManufacturers()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.GetAsync($"{path}/manufacturer").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null)
                    Manufacturers = resp.Content.ReadFromJsonAsync<ObservableCollection<Manufacturer>>().Result;
            }
        }

        private int DeleteProduct(Product product)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.DeleteAsync($"{path}/product/{product.Id}").Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp.IsSuccessStatusCode)
                    return 200;
                else
                    return 0;

            }
        }
        private void EditProduct(Product product)
        {
            
            if (ManufacturerId != null)
            {
                product.ManufacturerId=ManufacturerId.Id;
            }
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PutAsJsonAsync($"{path}/product", product).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }

            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
