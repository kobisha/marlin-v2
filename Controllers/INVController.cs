using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class INVController : ControllerBase
    {
        private readonly DataContext _context;

        public INVController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult CreateOrUpdateInvoice([FromBody] List<InvoiceData> invoiceDataList)
        {
            try
            {
                foreach (var invoiceData in invoiceDataList)
                {
                    var existingInvoice = _context.InvoiceHeaders.Include(i => i.InvoiceDetails)
                                                                .FirstOrDefault(i => i.InvoiceID == invoiceData.InvoiceID);

                    if (existingInvoice != null)
                    {
                        // Update fields of the existing invoice
                        existingInvoice.OrderID = invoiceData.OrderID;
                        existingInvoice.Date = invoiceData.Date;
                        existingInvoice.Number = invoiceData.Number;
                        existingInvoice.Amount = decimal.TryParse(invoiceData.Amount, out decimal amount) ? amount : (decimal?)null;
                        existingInvoice.WaybillNumber = invoiceData.WaybillNumber;

                        foreach (var product in invoiceData.Products)
                        {
                            var existingProduct = existingInvoice.InvoiceDetails.FirstOrDefault(p => p.Barcode == product.Barcode);
                            if (existingProduct != null)
                            {
                                // Update fields of the existing product
                                existingProduct.Unit = product.Unit;
                                existingProduct.Quantity = decimal.TryParse(product.Quantity, out decimal quantity) ? quantity : (decimal?)null;
                                existingProduct.Price = decimal.TryParse(product.Price, out decimal price) ? price : (decimal?)null;
                                existingProduct.Amount = decimal.TryParse(product.Amount, out decimal productAmount) ? productAmount : (decimal?)null;
                            }
                            else
                            {
                                // Create a new product
                                var newProduct = new InvoiceDetail
                                {
                                    InvoiceID = existingInvoice.InvoiceID,
                                    Barcode = product.Barcode,
                                    Unit = product.Unit,
                                    Quantity = decimal.TryParse(product.Quantity, out decimal quantity) ? quantity : (decimal?)null,
                                    Price = decimal.TryParse(product.Price, out decimal price) ? price : (decimal?)null,
                                    Amount = decimal.TryParse(product.Amount, out decimal productAmount) ? productAmount : (decimal?)null
                                };
                                _context.InvoiceDetails.Add(newProduct);
                            }
                        }
                    }
                    else
                    {
                        // Create a new invoice and products
                        var newInvoiceHeader = new InvoiceHeader
                        {
                            OrderID = invoiceData.OrderID,
                            InvoiceID = invoiceData.InvoiceID,
                            Date = invoiceData.Date,
                            Number = invoiceData.Number,
                            Amount = decimal.TryParse(invoiceData.Amount, out decimal amount) ? amount : (decimal?)null,
                            WaybillNumber = invoiceData.WaybillNumber,
                            PaymentDate = DateTimeOffset.MinValue,
                            InvoiceDetails = new List<InvoiceDetail>()
                        };

                        foreach (var product in invoiceData.Products)
                        {
                            var newInvoiceDetail = new InvoiceDetail
                            {
                                InvoiceID = newInvoiceHeader.InvoiceID,
                                Barcode = product.Barcode,
                                Unit = product.Unit,
                                Quantity = decimal.TryParse(product.Quantity, out decimal quantity) ? quantity : (decimal?)null,
                                Price = decimal.TryParse(product.Price, out decimal price) ? price : (decimal?)null,
                                Amount = decimal.TryParse(product.Amount, out decimal productAmount) ? productAmount : (decimal?)null
                            };
                            newInvoiceHeader.InvoiceDetails.Add(newInvoiceDetail);
                        }

                        _context.InvoiceHeaders.Add(newInvoiceHeader);
                    }
                }

                _context.SaveChanges();
                return Ok(new { message = "Invoices created or updated successfully." });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                var errorMessage = $"Failed to create or update invoices: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }
                return BadRequest(errorMessage);
            }
        }


    }
}
