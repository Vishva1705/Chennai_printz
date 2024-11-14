namespace BillingMVCProject.UIViewModels
{
    public class InvoiceFilterViewModel
    {
        public List<InvoiceCustomerDetail> Invoices { get; set; }
        public DateTime SelectedDate { get; set; }
        public string SearchTerm { get; set; }
    }
}
