
using BillingMVCProject.Models;
using BillingMVCProject.UIViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace BillingMVCProject.Controllers
{
    public class BillingController : Controller
    {

        private readonly billingContext _context;

        public BillingController(billingContext context)
        {
            _context = context;
        }

        // GET: Billings
        public async Task<IActionResult> Index()
        {
            var customerdetails = await _context.CustomersDetails.ToListAsync();
            return View(customerdetails);
        }

        [HttpPost]
        public IActionResult Create(CustomersDetail customer)
        {
            if (ModelState.IsValid)
            {
                _context.CustomersDetails.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Redirect to the customer list page
            }
            return View(customer); // Return to the create view if the model state is invalid
        }
        public IActionResult Edit(int id)
        {
            var product = _context.CustomersDetails.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return PartialView("_EditModal", product);
        }

        // POST: Products/Edit

        [HttpPost]
        public IActionResult Edit(CustomersDetail customer) // Accepting the entire customer model
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.CustomersDetails.Find(customer.CustomerId);
                if (existingCustomer != null)
                {
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.EmailId = customer.EmailId;
                    existingCustomer.PhoneNumber = customer.PhoneNumber;
                    existingCustomer.GstNumber = customer.GstNumber;

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(customer); // Return to the view with an error message if needed
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var customer = _context.CustomersDetails.Find(id);
            if (customer == null)
            {
                return NotFound(); // Return 404 if customer not found
            }

            _context.CustomersDetails.Remove(customer); // Remove the customer from the context
            _context.SaveChanges(); // Save changes to the database
            return RedirectToAction("Index"); // Redirect back to the Index action
        }
        [HttpGet]
        public IActionResult Invoice()
        {
            var customers = _context.CustomersDetails.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Address = c.Address,
                Email = c.EmailId,
                Phone = c.PhoneNumber
            }).ToList();

            var now = DateTime.Now;

            // Format the invoice number as "CPyyyyMMddHHmmssfff"
            string invoiceNumber = $"CP_{now:yyyyMMddHHmmssfff}";

            var viewModel = new InvoiceViewModel
            {
                Customers = customers, // Pass customers to the view
                InvoiceId = invoiceNumber

            };

            return View(viewModel);
            // Initialize the view model



        }
        [HttpPost]
        public IActionResult SaveInvoice(InvoiceViewModel model) // Create a view model for invoice
        {
            var invoiceDetail = new InvoiceDetail
            {
                InvoiceId = model.InvoiceId,
                CustomerId = model.CustomerId,
                Notes = model.Notes,
                Invoicedate = DateOnly.FromDateTime(model.InvoiceDate), 
                DeliveryCharge = model.DeliveryCharge,
                GstAmount = model.GstAmount,
                GrandTotal = model.GrandTotal,
                CreatedAt = DateTime.Now
            };

            _context.InvoiceDetails.Add(invoiceDetail);
            _context.SaveChanges();

            if(model.Items.Count > 0)
            {
                foreach (var item in model.Items)
                {
                    var invoiceItem = new Invoiceitemdetail // Adjust as per actual item class name
                    {
                        InvoiceId = invoiceDetail.InvoiceId, // Link item to the main invoice
                        Description = item.Description,
                        Size = item.Size,
                        Gsm = item.Gsm,
                        Quantity = item.Quantity,
                        Side = item.Side,
                        UnitPrice = item.UnitPrice,
                        Total = item.Quantity * item.UnitPrice
                    };

                    _context.Invoiceitemdetails.Add(invoiceItem); // Add each item to context
                }

                // Save all changes to the database
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Redirect to an appropriate page
        }
        [HttpGet]
        public IActionResult GetCustomerDetails(int customerId)
        {
            // Fetch the customer from the database based on the ID
            var customer = _context.CustomersDetails
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    Address = c.Address,
                    Email = c.EmailId,
                    Phone = c.PhoneNumber
                })
                .FirstOrDefault();

            // If the customer is not found, return a 404 error
            if (customer == null)
            {
                return NotFound();
            }

            // Return the customer data as JSON
            return Json(customer);
        }
        [HttpGet]
        public IActionResult InvoiceList()
        {
            var result = from invoice in _context.InvoiceDetails
                         join customer in _context.CustomersDetails
                         on invoice.CustomerId equals customer.CustomerId
                         select new InvoiceCustomerDetail
                         {
                             InvoiceId = invoice.InvoiceId,
                             CustomerId = invoice.CustomerId,
                             Notes = invoice.Notes,
                             Invoicedate = invoice.Invoicedate,
                             DeliveryCharge = invoice.DeliveryCharge,
                             GstAmount = invoice.GstAmount,
                             GrandTotal = invoice.GrandTotal,
                             CustomerName = customer.CustomerName,
                             
                         };

            var resultList = result.ToList();

            return View(resultList);
        }
        public IActionResult FilterInvoices(DateTime? invoiceDate)
        {
            // Retrieve all invoices from the database
            var result = from invoice in _context.InvoiceDetails
                         join customer in _context.CustomersDetails
                         on invoice.CustomerId equals customer.CustomerId
                         select new InvoiceCustomerDetail
                         {
                             InvoiceId = invoice.InvoiceId,
                             CustomerId = invoice.CustomerId,
                             Notes = invoice.Notes,
                             Invoicedate = invoice.Invoicedate,
                             DeliveryCharge = invoice.DeliveryCharge,
                             GstAmount = invoice.GstAmount,
                             GrandTotal = invoice.GrandTotal,
                             CustomerName = customer.CustomerName
                         };

            // Filter by date if invoiceDate is provided
            if (invoiceDate.HasValue)
            {
                var selectedDate = DateOnly.FromDateTime(invoiceDate.Value);
                result = result.Where(i => i.Invoicedate == selectedDate);
            }

            var invoiceCustomerDetails = result.ToList();

            return View("InvoiceList", invoiceCustomerDetails);
        }
        [HttpPost]
        public IActionResult ViewInvoiceDetails(string id, string flag)
        {
            //var invoiceDetails = (from inv in _context.InvoiceDetails
            //                      join item in _context.Invoiceitemdetails on inv.InvoiceId equals item.InvoiceId
            //                      join cust in _context.CustomersDetails on inv.CustomerId equals cust.CustomerId
            //                      where inv.InvoiceId == id
            //                      group new { inv, item, cust } by new
            //                      {
            //                          inv.InvoiceId,
            //                          inv.Invoicedate,
            //                          inv.DeliveryCharge,
            //                          inv.GstAmount
            //                      } into g
            //                      select new InvoiceDetailViewModel
            //                      {
            //                          InvoiceId = g.Key.InvoiceId,
            //                          Invoicedate = g.Key.Invoicedate,
            //                          DeliveryCharge = g.Key.DeliveryCharge,
            //                          GstAmount = g.Key.GstAmount,
            //                          flag = flag,
            //                          Items = g.Select(x => new InvoiceItemDetail
            //                          {
            //                              Gsm = x.item.Gsm,
            //                              Description = x.item.Description,
            //                              Quantity = x.item.Quantity,
            //                              Size = x.item.Size,
            //                              Side = x.item.Side,
            //                              UnitPrice = x.item.UnitPrice,
            //                              Total = x.item.Quantity * x.item.UnitPrice
            //                          }).ToList(),
            //                          Customers = g.Select(x => new CustomerDetailViewModel
            //                          {
            //                              CustomerId = x.cust.CustomerId,
            //                              CustomerName = x.cust.CustomerName,
            //                              PhoneNumber = x.cust.PhoneNumber,
            //                              EmailId = x.cust.EmailId,
            //                              Address = x.cust.Address,
            //                              GstNumber = x.cust.GstNumber
            //                          }).Distinct().ToList()
            //                      }).FirstOrDefault();
            var invoiceDetails = (from inv in _context.InvoiceDetails
                                  join item in _context.Invoiceitemdetails on inv.InvoiceId equals item.InvoiceId
                                  join cust in _context.CustomersDetails on inv.CustomerId equals cust.CustomerId
                                  where inv.InvoiceId == id
                                  group new { inv, item, cust } by new
                                  {
                                      inv.InvoiceId,
                                      inv.Invoicedate,
                                      inv.DeliveryCharge,
                                      inv.GstAmount,
                                      inv.GrandTotal,
                                      cust.CustomerName,
                                      cust.PhoneNumber,
                                      cust.EmailId,
                                      cust.Address,
                                      cust.GstNumber,
                                      cust.CustomerId
                                  } into g
                                  select new InvoiceDetailViewModel
                                  {
                                      InvoiceId = g.Key.InvoiceId,
                                      Invoicedate = g.Key.Invoicedate,
                                      DeliveryCharge = g.Key.DeliveryCharge,
                                      GstAmount = g.Key.GstAmount,
                                      
                                      Items = g.Select(x => new InvoiceItemDetail
                                      {
                                          Gsm = x.item.Gsm,
                                          ItemId= x.item.ItemId,
                                          Description = x.item.Description,
                                          Quantity = x.item.Quantity,
                                          Size = x.item.Size,
                                          Side = x.item.Side,
                                          UnitPrice = x.item.UnitPrice,
                                          Total = x.item.Quantity * x.item.UnitPrice
                                      }).ToList(),
                                      Customer_id = g.Key.CustomerId, // Capture selected Customer ID
                                      CustomerName = g.Key.CustomerName,
                                      PhoneNumber = g.Key.PhoneNumber,
                                      EmailId = g.Key.EmailId,
                                      Address = g.Key.Address,
                                      GstNumber = g.Key.GstNumber,
                                      flag=flag,
                                      GrandTotal =g.Key.GrandTotal,
                                      Customers = _context.CustomersDetails.Select(c => new CustomerDetailViewModel
                                      {
                                          CustomerId = c.CustomerId,
                                          CustomerName = c.CustomerName,
                                          PhoneNumber = c.PhoneNumber,
                                          EmailId = c.EmailId,
                                          Address = c.Address,
                                          GstNumber = c.GstNumber
                                      }).ToList() // Fetch all customers for the dropdown
                                  }).FirstOrDefault();

            return PartialView("_InvoiceDetailsPartial", invoiceDetails);

        }
        [HttpPost]
        public IActionResult UpdateInvoice(InvoiceViewModel model)
        {
            if (model != null)
            {
                var CustomerIdCheck = _context.InvoiceDetails.Where(x => x.InvoiceId == model.InvoiceId).ToList();
                if (model.CustomerId != CustomerIdCheck[0].CustomerId)
                {
                    string updateSql = "UPDATE invoice_details " +
                     "SET CustomerId = {0}, DeliveryCharge = {1}, GstAmount = {2}, GrandTotal = {3} " +  // Removed extra comma here
                     "WHERE InvoiceDate = {4} AND InvoiceId = {5};";

                    _context.Database.ExecuteSqlRaw(updateSql, model.CustomerId, model.DeliveryCharge, model.GstAmount, model.GrandTotal, model.InvoiceDate, model.InvoiceId);

                }
                if (model.Items.Count > 0)
                {
                    foreach (var item in model.Items)
                    {
                        string updateLineItems = "UPDATE invoiceitemdetails " +
                         "SET Description = @Description, Size = @Size, Gsm = @Gsm, " +
                         "Quantity = @Quantity, Side = @Side, UnitPrice = @UnitPrice, Total = @Total " +
                         "WHERE InvoiceId = @InvoiceId AND ItemId = @ItemId;";

                        // Execute the update command using parameters
                        _context.Database.ExecuteSqlRaw(updateLineItems,
                            new MySqlParameter("@Description", item.Description),
                            new MySqlParameter("@Size", item.Size),
                            new MySqlParameter("@Gsm", item.Gsm),
                            new MySqlParameter("@Quantity", item.Quantity),
                            new MySqlParameter("@Side", item.Side),
                            new MySqlParameter("@UnitPrice", item.UnitPrice),
                            new MySqlParameter("@Total", item.TotalValues),
                            new MySqlParameter("@InvoiceId", model.InvoiceId),
                            new MySqlParameter("@ItemId", item.Itemid));
                        // string UpdateLineItems = "UPDATE invoiceitemdetails " +
                        //"SET Description = {0}, Size = {1}, Gsm = {2}, Quantity = {3} ,Side ={4},UnitPrice= {5},Total={6}" +  // Removed extra comma here
                        //"WHERE InvoiceId = {7} AND ItemId = {8};";
                        // _context.Database.ExecuteSqlRaw(UpdateLineItems, item.Description, item.Size, item.Gsm, item.Quantity, item.Side, item.UnitPrice, item.Total, model.InvoiceId, item.Itemid);

                    }
                }




            }
            return RedirectToAction("InvoiceList");
        }
        

    }
}
