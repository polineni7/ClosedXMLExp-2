﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class AdmServiceContractsPackingType
    {
        public int PackingTypeId { get; set; }
        public string PackingName { get; set; }
        public double? DefaultPerCargoAddition { get; set; }
    }
}
