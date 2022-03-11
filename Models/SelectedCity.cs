﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class SelectedCity
    {
        public long SelectedCityId { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
