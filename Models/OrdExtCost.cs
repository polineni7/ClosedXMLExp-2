using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class OrdExtCost
    {
        public string OrdExtCostId { get; set; }
        public string OrdDivId { get; set; }
        public string Descr { get; set; }
        public decimal? Amt { get; set; }
        public double? Pt { get; set; }
    }
}
