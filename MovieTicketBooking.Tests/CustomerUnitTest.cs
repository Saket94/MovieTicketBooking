using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.DataAccessLayer;
using MovieTicketBooking.Helpers;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MovieTicketBooking.Tests
{
    [TestClass]
    public class CustomerUnitTest
    {
        /// <summary>
        /// Global Declarations
        /// </summary>
        private IGenericRepository<tblCustomer> repository = null;

        /// <summary>
        /// Declaration of CustomerUnitTest constructor and assigning GenericRepository to previously defined repository;
        /// </summary>
        public CustomerUnitTest()
        {
            this.repository = new GenericRepository<tblCustomer>();
        }

        /// <summary>
        /// Declaration of TestContext property, which will be used to access records from xml file.
        /// </summary>
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        /// <summary>
        /// This method is used to get the customer id from the xml file and test it.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\CustomerTestCases.xml", "tbl_Customer", DataAccessMethod.Sequential)]
        public void GetCustomer()
        {
            // Arrange
            CustomerController controller = new CustomerController(this.repository);
            // Act
            string custEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            Expression<Func<tblCustomer, bool>> f2 = (p => p.Email == custEmail);
            List<tblCustomer> customers = new List<tblCustomer>(repository.GetMatched(f2));
            var chkCustomersId = (from p in customers select p.id).FirstOrDefault();
            int customerId = Convert.ToInt32(chkCustomersId);

            JsonResult result = controller.GetCustomer(customerId) as JsonResult;
            // Assert
            Assert.IsNotNull(result.Data);
        }

        /// <summary>
        /// This method is used to get the customer email from the xml file and test it.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\CustomerTestCases.xml", "tbl_Customer", DataAccessMethod.Sequential)]
        public void GetCustomerEmail()
        {
            // Arrange
            CustomerController controller = new CustomerController(this.repository);
            // Act
            string customerEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            JsonResult result = controller.GetCustomerEmail(customerEmail) as JsonResult;
            // Assert
            Assert.IsNotNull(result.Data);
        }
    }
}
