using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
    public class DetailTranferBarcodeViewModel
    {
        public int TransferId { get; set; }
        public int ProductId { get; set; }
        public string PreviousStore { get; set; }
        public string CurrentStore { get; set; }
        public string Barcode { get; set; }
        public string FunctionTypeRecipt { get; set; }
        public string FunctionTypeDraft { get; set; }
        public DateTime? CreateDate { get; set; } = null;
        public string CreateUser { get; set; }
        public string DateTransfer { get; set; }
        public decimal Weight { get; set; }
        public string IsRegToOrderColor { get; set; }
        public string BarcodeDescription { get; set; }
        public string TypeCloth { get; set; }
        public string NameCotton { get; set; }
        public string TransferDescription { get; set; }
        public int CodeCommondity { get; set; }
        public int DeviceId { get; set; }
        public int CodeStore { get; set; }
        public string Number { get; set; }
        //public int TransferId { get; set; }
    }
}
