using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
    class ReportDeviceViewModel
    {
        public int DeviceId { get; set; }
        public string NameDevice { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string TagCode { get; set; }
        public string Date { get; set; }
        public DateTime CreateDate { get; set; }
        public string IsDeffectiveText { get; set; }
        public bool IsDeffective { get; set; }
    }
}
