using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorClient.ViewModel
{
    public class MainViewModel
    {
        public ManufacturerViewModel ManufacturerTab { get; set; }
        public ProductViewModel ProductTab { get; set; }
        public EditProductViewModel EditProductTab { get; set; }

        public MainViewModel() 
        {
            ManufacturerTab = new ManufacturerViewModel();
            ProductTab = new ProductViewModel();
            EditProductTab = new EditProductViewModel();
        }
    }
}
