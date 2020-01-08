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
    public class BookingUnitTest
    {
        /// <summary>
        /// Global Declarations
        /// </summary>
        private IGenericRepository<tblCustomer> customerRepository = null;
        private IGenericRepository<tblBooking> bookingRepository = null;
        private IGenericRepository<tblSeat> seatRepository = null;

        /// <summary>
        /// Declaration of BookingUnitTest constructor and assigning GenericRepository to previously defined repository;
        /// </summary>
        public BookingUnitTest()
        {
            this.customerRepository = new GenericRepository<tblCustomer>();
            this.bookingRepository = new GenericRepository<tblBooking>();
            this.seatRepository = new GenericRepository<tblSeat>();
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
        /// This method is used to check the seat availabilty
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void CheckSeatAvailabilty()
        {
            // Arrange
            BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
            int SeatNo = Convert.ToInt32(TestContext.DataRow["SeatNo"]);
            bool result = controller.CheckSeatAvailabilty(SeatNo);
            //Assert
            if (result == true)
                Assert.IsTrue(result);
            else
                Assert.IsFalse(result);
        }

        /// <summary>
        /// This method is used to check for the existing customer
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void CheckForExistingCustomer()
        {
            // Arrange
            BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
            string custEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            tblCustomer result = controller.CheckForExistingCustomer(custEmail) as tblCustomer;
            //Assert
            if (result != null)
                Assert.IsNotNull(result.Email);
            else
                Assert.IsNull(result);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void VerifyPassCode()
        {
            // Arrange
            BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
            string custPassCode = Convert.ToString(TestContext.DataRow["CustomerPassCode"]);

            Expression<Func<tblBooking, bool>> f2 = (p => p.passCode == custPassCode);
            List<tblBooking> bookings = new List<tblBooking>(bookingRepository.GetMatched(f2));
            var custId = (from p in bookings select p.cust_id).FirstOrDefault();
            bool result = controller.VerifyPassCode(custPassCode, Convert.ToInt32(custId));
            //Assert
            if (result == true)
                Assert.IsTrue(result);
            else
                Assert.IsFalse(result);
        }

        /// <summary>
        /// This method is used to get the booking details
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void GetBooking()
        {
            // Arrange
            BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
            // Act
            string custEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            string custPassCode = Convert.ToString(TestContext.DataRow["CustomerPassCode"]);
            JsonResult result = controller.GetBooking(custEmail, custPassCode) as JsonResult;
            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// This method is used to get the customer details from the xml file and test it.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void BookSeats()
        {
            // Arrange
            BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
            // Act
            string selectedSeatsHd = Convert.ToString(TestContext.DataRow["CustomerSelectedSeatsHd"]);
            string customerFullName = Convert.ToString(TestContext.DataRow["CustomerFullName"]);
            string customerEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            string customerPassCode = Convert.ToString(TestContext.DataRow["CustomerPassCode"]);

            if (!string.IsNullOrEmpty(selectedSeatsHd) && !string.IsNullOrEmpty(customerFullName) && !string.IsNullOrEmpty(customerEmail)
                && !string.IsNullOrEmpty(customerPassCode))
            {
                FormCollection form = new FormCollection();  //CustomerSelectedSeatsHd
                form.Add("selectedSeatsHd", Convert.ToString(TestContext.DataRow["CustomerSelectedSeatsHd"]));
                form.Add("txtFullName", Convert.ToString(TestContext.DataRow["CustomerFullName"]));
                form.Add("txtEmail", Convert.ToString(TestContext.DataRow["CustomerEmail"]));
                form.Add("txtPassCode", Convert.ToString(TestContext.DataRow["CustomerPassCode"]));

                //JsonResult result = controller.BookSeats(CreateTestFormCollection()) as JsonResult;
                JsonResult result = controller.BookSeats(form) as JsonResult;
                Response response = (Response)result.Data;
                //Assert
                if (response.success == true)
                    Assert.IsTrue(response.success);
                else
                    Assert.IsFalse(response.success);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }

        /// <summary>
        /// This method is used for unbooking the seats
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"|DataDirectory|\TestCases\BookingTestCases.xml", "tbl_Booking", DataAccessMethod.Sequential)]
        public void Unbooking()
        {
            string selectedSeatsHd = Convert.ToString(TestContext.DataRow["CustomerSelectedSeatsHd"]);
            string customerFullName = Convert.ToString(TestContext.DataRow["CustomerFullName"]);
            string customerEmail = Convert.ToString(TestContext.DataRow["CustomerEmail"]);
            string customerPassCode = Convert.ToString(TestContext.DataRow["CustomerPassCode"]);

            if (!string.IsNullOrEmpty(selectedSeatsHd) && !string.IsNullOrEmpty(customerFullName) && !string.IsNullOrEmpty(customerEmail)
                && !string.IsNullOrEmpty(customerPassCode))
            {
                List<int> unBookedSeatList = new List<int>();
                string strUnbookedSeats = Convert.ToString(TestContext.DataRow["CustomerSelectedSeatsHd"]);
                var finalUnbookedSeat = strUnbookedSeats.Split(',');
                foreach (var item in finalUnbookedSeat)
                {
                    unBookedSeatList.Add(Convert.ToInt32(item));
                }
                string custPassCode = Convert.ToString(TestContext.DataRow["CustomerPassCode"]);

                Expression<Func<tblBooking, bool>> f2 = (p => p.passCode == custPassCode);
                List<tblBooking> bookings = new List<tblBooking>(bookingRepository.GetMatched(f2));
                var custId = (from p in bookings select p.cust_id).FirstOrDefault();

                // Arrange
                BookingController controller = new BookingController(this.customerRepository, this.bookingRepository, this.seatRepository);
                // Act
                JsonResult result = controller.Unbooking(Convert.ToInt32(custId), unBookedSeatList, custPassCode) as JsonResult;
                Response response = (Response)result.Data;
                // Assert
                if (response.success == true)
                    Assert.IsTrue(response.success);
                else
                    Assert.IsFalse(response.success);
            }
            else
            {
                Assert.IsFalse(false);
            }
        }
    }
}
