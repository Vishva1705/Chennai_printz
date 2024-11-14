using System;
using System.Collections.Generic;

namespace BillingMVCProject.Models
{
    public partial class Invoiceitemdetail
    {
        public int ItemId { get; set; }
        public string? InvoiceId { get; set; }
        public string Description { get; set; } = null!;
        public string? Size { get; set; }
        public string? Gsm { get; set; }
        public int Quantity { get; set; }
        public string? Side { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalValues { get; set; }
    }
}
