using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCLOR.Models
{
    public class Machine
    {
        public int ID { get; set; }
        public int? Code { get; set; }
        public string NameMachine { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }
}
