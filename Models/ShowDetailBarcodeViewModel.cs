﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
  public class ShowDetailBarcodeViewModel
    {
        public string BarCode { get; set; }
        public string ClothName { get; set; }
        public decimal Weight { get; set; }
        public string Weaver { get; set; }
        public string Machine { get; set; }
        public string Description { get; set; }
        public decimal Purity { get; set; }
        public string Shift { get; set; }
        public string Date { get; set; }
    }
}