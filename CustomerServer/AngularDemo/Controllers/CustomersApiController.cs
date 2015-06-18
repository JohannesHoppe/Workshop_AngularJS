using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AngularDemo.Models;

namespace AngularDemo.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class CustomersApiController : ApiController
    {
        private CustomerList db;

        public CustomersApiController()
        {
            db = new CustomerList();
        }

        // GET: api/CustomersApi
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers.AsQueryable();
        }

        // GET: api/CustomersApi/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.First(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/CustomersApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            Customer customerOld = db.Customers.First(c => c.Id == id);
            customerOld.FirstName = customer.FirstName;
            customerOld.LastName = customer.LastName;
            customerOld.Mail = customer.Mail;

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CustomersApi
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/CustomersApi/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.First(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }
    }
}