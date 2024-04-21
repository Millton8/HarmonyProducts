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
    public class ProductViewModel : INotifyPropertyChanged
    {
        public ProductViewModel()
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
        private Product product = new();
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }


        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      int code = 0;
                      code = AddProduct(Product);

                      if (code == 200)
                          GetProducts();
                      else
                          MessageBox.Show("Произошла ошибка\n Данные не добалены");

                  }));
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
        /// <summary>
        /// Получаем список производителей для выпадающего списка
        /// </summary>
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

        private int AddProduct(Product product)
        {
            //Добавляем Id производителя который выбрали списком
            product.ManufacturerId=ManufacturerId.Id;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage? resp = null;
                try
                {
                    resp = client.PostAsJsonAsync($"{path}/product", product).Result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось подключиться к серверу");
                }
                if (resp != null && resp.IsSuccessStatusCode)
                {

                    Products = resp.Content.ReadFromJsonAsync<ObservableCollection<Product>>().Result;
                    return 200;
                }
                else
                    return 0;

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
