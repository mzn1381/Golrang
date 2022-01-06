using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
    public class FillDetailBarcodeViewModel
    {
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string NameDevice { get; set; }
        public string Barcode { get; set; }
        public string ClothName { get; set; }
    }
}
