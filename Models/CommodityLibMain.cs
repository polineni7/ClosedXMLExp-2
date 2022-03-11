using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class CommodityLibMain
    {
        public CommodityLibMain()
        {
            CommodityLibSubs = new HashSet<CommodityLibSub>();
        }

        public string MainId { get; set; }
        public string MainCommodity { get; set; }

        public virtual ICollection<CommodityLibSub> CommodityLibSubs { get; set; }
    }
}
