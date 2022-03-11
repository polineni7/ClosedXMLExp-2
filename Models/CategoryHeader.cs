using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class CategoryHeader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SortOrder { get; set; }
    }
}
