namespace BillingMVCProject.UIViewModels
{
    public class InvoiceDetailViewModel
    {
        public string InvoiceId { get; set; }
        public DateOnly Invoicedate { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal GstAmount { get; set; }
        public List<InvoiceItemDetail> Items { get; set; } // Change from single item to a list
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string GstNumber { get; set; }
        public InvoiceItemDetail Item { get; internal set; }

        public CustomerDetailViewModel customerDetailViewModel { get; set; }

        public string flag { get; set; }

        public decimal GrandTotal { get; set; }

        public int Customer_id { get;set; }
        public List<CustomerDetailViewModel> Customers { get; internal set; }
    }

    // Define InvoiceItemDetail class if needed
    public class InvoiceItemDetail
    {
        public string Description { get; set; }

        public int ItemId { get; set; }
        public string Size { get; set; }
        public string Gsm { get; set; }
        public int Quantity { get; set; }
        public string Side { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
    public class CustomerDetailViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string GstNumber { get; set; }
    }
}
