using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdministratorClient.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public string Category { get; set; }
        private float price;
        public float Price 
        { 
            get { return price; }
            set 
            {
                if (value < 0)
                    price = 0;
                price = value;
            }
        }
        public float? ConditionPrice { get; set; } = null;
        public short Bonus { get; set; } = 0;
        public bool isRecept { get; set; } = false;
        public string[] Images { get; set; } = ["PICTURE.jpg"];
        private int retailCount;
        public int RetailCount
        {
            get { return retailCount; }
            set
            {
                if (value < 0)
                    retailCount = 0;
                retailCount = value;
            }
        }

        public string? Description { get; set; } = null;

        





    }
}
