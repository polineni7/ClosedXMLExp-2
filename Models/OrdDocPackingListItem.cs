using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class OrdDocPackingListItem
    {
        public int ItemId { get; set; }
        public string OrdDivId { get; set; }
        public short PieceNo { get; set; }
        public int? ItemOrder { get; set; }
        public double? Qty { get; set; }
        public string Description { get; set; }
    }
}
