﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class OrdDocPakListSubItemsSerial
    {
        public string SerialNum { get; set; }
        public long? Id { get; set; }

        public virtual OrdDocPakListSubItem IdNavigation { get; set; }
    }
}
