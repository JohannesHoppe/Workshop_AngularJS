using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using AngularDemo.Models;

namespace AngularDemo.Controllers
{
    /// <summary>
    /// This is an OData v3 controller!!
    /// </summary>
    public class CustomersController : ODataController
    {
        private CustomerList db;

        public CustomersController()
        {
            db = new CustomerList();
        }

        // GET: odata/Customers
        [EnableQuery]
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers.AsQueryable();
        }

        [HttpPost]
        public IHttpActionResult Purchase([FromODataUri] int key, ODataActionParameters parameters)
        {
            int amount = (int)parameters["AmountOfShoes"];
            var customer = db.Customers.First(c => c.Id == key);
            var invoices = CustomerService.PurchaseShoesAndSendMail(customer, amount);

            if (!invoices.Any())
            {
                return NotFound();
            }

            return Ok(invoices);
        }


        // GET: odata/Customers(5)
        [EnableQuery]
        public SingleResult<Customer> GetCustomer([FromODataUri] int key)
        {
            var x = db.Customers.Where(customer => customer.Id == key).AsQueryable();
            return new SingleResult<Customer>(x);
        }

        // PUT: odata/Customers(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Customer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = db.Customers.First(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            patch.Put(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customer);
        }

        // POST: odata/Customers
        public IHttpActionResult Post(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return Created(customer);
        }

        // PATCH: odata/Customers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Customer> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = db.Customers.First(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            patch.Patch(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(customer);
        }

        // DELETE: odata/Customers(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Customer customer = db.Customers.First(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Customers(5)/Invoices
        [EnableQuery]
        public IQueryable<Invoice> GetInvoices([FromODataUri] int key)
        {
            return db.Customers.First(m => m.Id == key).Invoices.AsQueryable();
        }

        private bool CustomerExists(int key)
        {
            return db.Customers.Count(e => e.Id == key) > 0;
        }
    }
}
