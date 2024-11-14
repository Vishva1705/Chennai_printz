namespace BillingMVCProject.UIViewModels
{
    public class InvoiceViewModel
    {
        public int CustomerId { get; set; } // ID of the customer selected

        
        public List<InvoiceItemViewModel> Items { get; set; } = new List<InvoiceItemViewModel>(); // List of items on the invoice

        public DateTime InvoiceDate { get; set; }
        public string InvoiceId { get; set; }

        public List<CustomerViewModel> Customers { get; set; }
        public string Notes { get; set; } // Notes for the invoice

        public decimal DeliveryCharge { get; set; } // Delivery charge

        public decimal GstAmount { get; set; } // GST amount

        public decimal GrandTotal { get; set; } // Total amount due
        public DateTime CreatedAt { get; internal set; }
    }

    public class InvoiceItemViewModel
    {
        public int Itemid { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string Gsm { get; set; }
        public int Quantity { get; set; }
        public string Side { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public decimal TotalValues { get; set; } // Calculate total based on quantity and unit price
        
        
    }
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
