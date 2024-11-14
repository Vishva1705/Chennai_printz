using System;
using System.Collections.Generic;

namespace BillingMVCProject.Models
{
    public partial class CustomersDetail
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public string? Address { get; set; }
        public string? GstNumber { get; set; }
    }
}
