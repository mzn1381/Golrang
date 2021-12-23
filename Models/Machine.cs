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
        public string Specstechnical { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string DeviceMark { get; set; }
        public int Gap { get; set; }
        public decimal teeny { get; set; }
        public decimal Area { get; set; }
        public int YarnType { get; set; }
        public decimal RoundStop { get; set; }
        public int TextureLimit { get; set; }
        public Int64 FabricType { get; set; }
        public string YarnTypeName { get; set; }
        public string FabricTypeName { get; set; }
    }
}
