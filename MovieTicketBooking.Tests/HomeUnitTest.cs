using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieTicketBooking;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.DataAccessLayer;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Tests
{
    [TestClass]
    public class HomeUnitTest
    {
        /// <summary>
        /// Global Declarations
        /// </summary>
        private IGenericRepository<tblSeat> repository = null;

        /// <summary>
        /// Declaration of HomeUnitTest constructor and assigning GenericRepository to previously defined repository;
        /// </summary>
        public HomeUnitTest()
        {
            this.repository = new GenericRepository<tblSeat>();
        }

        /// <summary>
        /// This method is used to test the index method of homecontroller.
        /// </summary>
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(this.repository);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
