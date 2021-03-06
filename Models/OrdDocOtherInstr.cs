using System;
using System.Collections.Generic;

#nullable disable

namespace ClosedXMLExp.Models
{
    public partial class OrdDocOtherInstr
    {
        public string OrdDivId { get; set; }
        public byte? Exist { get; set; }
        public DateTime? FormDate { get; set; }
        public string ShipCompany { get; set; }
        public string ShipFullName { get; set; }
        public string Instructions { get; set; }
        public string Shipper { get; set; }
        public string Notify { get; set; }
        public string Consignee { get; set; }
        public string Markings { get; set; }
        public string NoTypePkgs { get; set; }
        public string Description { get; set; }
        public string FootAdvise { get; set; }
        public string FootNb { get; set; }
        public string StatusDb { get; set; }
        public double? TotalAmount { get; set; }
        public string FxCurrencyCode { get; set; }
        public double? FxRate { get; set; }
        public double? FxAmt { get; set; }
        public string PaymentTerm { get; set; }
        public double? FiServiceCost { get; set; }
        public string ChgEmId { get; set; }
        public DateTime? ChgLog { get; set; }
        public DateTime? SubmittedToAccounting { get; set; }
        public string Invoice { get; set; }
    }
}
