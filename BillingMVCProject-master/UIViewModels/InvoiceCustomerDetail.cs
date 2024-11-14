namespace BillingMVCProject.UIViewModels
{
    public class InvoiceCustomerDetail
    {
        public string InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public string? Notes { get; set; }
        public DateOnly Invoicedate { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }

        // Properties from CustomerDetails
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }

        public DateTime? InvoiceDate { get; set; }
    }
}
