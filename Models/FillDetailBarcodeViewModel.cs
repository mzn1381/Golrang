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
        public int JoinShift { get; set; }
        public string NameOperator1 { get; set; }
        public string OperatorTag1 {get;set;}
        public decimal PurityOperator1 { get; set; }
        public string NameOperator2 { get; set; }
        public string OperatorTag2 { get; set; }
        public decimal PurityOperator2 { get; set; }
        //public string Weaver { get; set; }
        //public decimal Purity { get; set; }
        public string Shift { get; set; }
        public string Date { get; set; }
        /// <summary>
        /// کد مربوط به ClothType
        /// </summary>
        public string CodeCommodity { get; set; }
    }
}
