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
        public string CottonName { get; set; }
        public string Weaver { get; set; }
        public decimal Purity { get; set; }
        public string Shift { get; set; }
        public string Date { get; set; }
    }
}
