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
using System.Windows.Data;
using WorkerClient.Model;

namespace WorkerClient.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            GetProducts();
            productView = CollectionViewSource.GetDefaultView(Products);
            productView.Filter = FilterPerson;

        }
        private string filterCategory;
        private string filterName;
        private const string path = "http://localhost:5125";
        private ICollectionView productView;
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

        public string FilterCategory
        {
            get { return filterCategory; }
            set
            {
                if (filterCategory != value)
                {
                    filterCategory = value;
                    OnPropertyChanged(nameof(FilterCategory));
                    productView.Refresh();
                }
            }
        }


        public string FilterName
        {
            get { return filterName; }
            set
            {
                if (filterName != value)
                {
                    filterName = value;
                    OnPropertyChanged(nameof(FilterName));
                    productView.Refresh();
                }
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
        private bool FilterPerson(object obj)
        {

            if (string.IsNullOrEmpty(FilterCategory) && string.IsNullOrEmpty(FilterName))
                return true;


            if (obj is Product product)
            {
                bool firstNameMatches = string.IsNullOrEmpty(FilterName) || product.Name.ToLower().Contains(FilterName.ToLower());
                bool lastNameMatches = string.IsNullOrEmpty(FilterCategory) || product.Category.ToLower().Contains(FilterCategory.ToLower());
                //bool selectNameMatches = string.IsNullOrEmpty(SelectedName) || person.FirstName.Contains(SelectedName);


                return firstNameMatches && lastNameMatches;
            }


            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
