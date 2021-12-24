using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
   public class ProductSourceViewModel
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public string Barcode { get; set; }
        public string Machine { get; set; }
        public string Operator { get; set; }
        public string Date { get; set; }
        public int Time { get; set; }
        public int ClothType { get; set; }
        public int CottonType { get; set; }
        public int TimeLastProduct { get; set; }
        public int TimeLastShift { get; set; }
        public int Numberweave { get; set; }
        public int weight { get; set; }
        public string ReportDescription { get; set; }
        public int shift { get; set; }
        public string UserSabt { get; set; }
        public string DateSabt { get; set; }
        public string UserEdit { get; set; }
        public string DateEdit { get; set; }

    }
}
