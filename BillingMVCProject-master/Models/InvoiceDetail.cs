using System;
using System.Collections.Generic;

namespace BillingMVCProject.Models
{
    public partial class InvoiceDetail
    {
        public string InvoiceId { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? Notes { get; set; }
        public DateOnly Invoicedate { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
