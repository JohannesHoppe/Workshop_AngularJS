using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularDemo.Models;
using AutoPoco;
using AutoPoco.DataSources;
using AutoPoco.Engine;


namespace AngularDemo.Controllers
{
    public class ResetController : ApiController
    {
        private CustomerList db;

        public ResetController()
        {
            db = new CustomerList();
        }

        /// <summary>
        /// Resets the demo data to its initial state
        /// </summary>
        /// <returns>200 and a text</returns>
        public HttpResponseMessage Get()
        {
            Reset();
            return Request.CreateResponse(HttpStatusCode.OK, "Demo Data was resetted!");
        }

        public void Reset()
        {
            var DemoData = GenerateDemoCustomers();

            db.Customers.Clear();

            int i = 1;
            int n = 1;

            foreach (var customer in DemoData)
            {
                customer.Id = ++i;
                db.Customers.Add(customer);

                var invoices = GenerateDemoInvoices(customer.Id);

                foreach (var invoice in invoices)
                {
                    invoice.Id = ++n;
                    invoice.Customer = customer;
                    invoice.CustomerId = customer.Id;
                    customer.Invoices.Add(invoice);
                }
            }
        }

        private static IEnumerable<Customer> GenerateDemoCustomers()
        {
            IGenerationSessionFactory factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c => c.UseDefaultConventions());
                x.AddFromAssemblyContainingType<Customer>();

                x.Include<Customer>()
                    .Setup(c => c.FirstName).Use<FirstNameSource>()
                    .Setup(c => c.LastName).Use<LastNameSource>()
                    .Setup(c => c.Mail).Use<EmailAddressSource>()
                    .Setup(c => c.DateOfBirth).Use<DateOfBirthSource>();
            });

            IGenerationSession session = factory.CreateSession();
            return session.List<Customer>(100).Get();
        }

        private static IEnumerable<Invoice> GenerateDemoInvoices(int customerId)
        {
            Random rnd = new Random(customerId);
            int amountOfInvoices = rnd.Next(0, 10);

            IGenerationSessionFactory factory = AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c => c.UseDefaultConventions());
                x.AddFromAssemblyContainingType<Invoice>();

                x.Include<Invoice>()
                    .Setup(c => c.Amount).Use<DecimalSource>(1m, 200m);
            });

            IGenerationSession session = factory.CreateSession();
            return session.List<Invoice>(amountOfInvoices).Get();
        }
    }
}
