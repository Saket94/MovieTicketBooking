using MovieTicketBooking.DataAccessLayer;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Linq.Expressions;
using System.Net;

namespace MovieTicketBooking.Controllers
{
    public class BookingController : Controller
    {
        /// <summary>
        /// Declaration of IRepository Instances for CRUD Operations
        /// </summary>
        private IGenericRepository<tblCustomer> customerRepository = null;
        private IGenericRepository<tblBooking> bookingRepository = null;
        private IGenericRepository<tblSeat> seatRepository = null;

        public BookingController()
        {
            this.customerRepository = new GenericRepository<tblCustomer>();
            this.bookingRepository = new GenericRepository<tblBooking>();
            this.seatRepository = new GenericRepository<tblSeat>();
        }

        public BookingController(IGenericRepository<tblCustomer> customerRepository, IGenericRepository<tblBooking> bookingRepository, IGenericRepository<tblSeat> seatRepository)
        {
            this.customerRepository = customerRepository;
            this.bookingRepository = bookingRepository;
            this.seatRepository = seatRepository;
        }

        /// <summary>
        /// This mehtod is used to book the seats by the user.
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BookSeats(FormCollection fc)
        {
            SendEmail objSendEmail = new SendEmail();
            //Gathering values from Form
            List<int> seats = fc["selectedSeatsHd"].Split(',').Select(int.Parse).ToList();
            string name = fc["txtFullName"];
            string email = fc["txtEmail"];
            string passCode = fc["txtPassCode"];

            //Creating response Object
            Helpers.Response objResponse = new Helpers.Response();
            try
            {
                //Creating customer Object and assigning values to it
               
                tblCustomer customer = new tblCustomer();
                
                if ((customer = CheckForExistingCustomer(email)) == null)
                {
                    customer = new tblCustomer();
                    customer.FullName = name;
                    customer.Email = email;
                   
                    //Insert Customer object to DB
                    customerRepository.Insert(customer);
                    customerRepository.Save();
                }
       
                
                foreach (var seat in seats)
                {
                    try
                    {
                        //Cheking Seat Availibility
                        if (CheckSeatAvailabilty(seat))
                        {
                            //Fetch seat to update its status
                            tblSeat objSeat = seatRepository.GetById(seat);
                            //Update status to false (ie Unavailable)
                            objSeat.Status = false;
                            seatRepository.Update(objSeat);
                        }
                        else
                        {
                            //Throw exception if Seat is booked by other user meanwhile
                            throw new DbUpdateConcurrencyException();
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                        //////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // This Exception will be thrown on two cases -
                        // 1. False return by method : checkSeatAvailabilty(seat), This does not check concurrency but 
                        //    to check availibilty meanwhile user selects seats and confirms the seats.
                        // 2. Realtime Concurrency Exception by Optimistic Concurrency Control approch applied on DBMS and ORM (EF)
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////

                        objResponse.success = false;
                        objResponse.msg = "One or more selected seats were booked by other person.";
                        return Json(objResponse);
                    }
                    // Add booking record to DB 
                    tblBooking objBooking = new tblBooking() { seat_Number = seat, cust_id = customer.id, status = true , passCode = passCode };
                    bookingRepository.Insert(objBooking);
                }
                bookingRepository.Save();
                seatRepository.Save();

                //Send Mail to User for confirmation
                if (objSendEmail.SendBookingMail(seats, name, passCode, email))
                {
                    objResponse.success = true;
                    objResponse.msg = "Cinema ticket booking saved successfully. Please check your email-id for the booking details.";
                }
                else
                {
                    objResponse.success = false;
                    objResponse.msg = "Something went wrong. Error sending mail.";
                }
                return Json(objResponse);
            }
            catch (Exception ex)
            {
                //Handling Exception and sending appropriate response to client.
                objResponse.success = false;
                objResponse.msg = "Error :" + ex.Message;
                return Json(objResponse);
            }
        }

        /// <summary>
        /// This method is used to un-booked the previously booked seats
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="seats"></param>
        /// <param name="passCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Unbooking(int custId, List<int> seats, string passCode)
        {
            //Creating Response Object
            SendEmail objSendEmail = new SendEmail();
            Helpers.Response objResponse = new Helpers.Response();
            if (VerifyPassCode(passCode, custId) == false)
            {
                objResponse.success = false;
                objResponse.msg = "Problem Un-booking seats. Error  Incorrect passcode.";
                return Json(objResponse);
            }

            try
            {
                foreach (int seat in seats)
                {
                    // Get seat by Id and set its availibility true
                    tblSeat objSeat = seatRepository.GetById(seat);
                    objSeat.Status = true;

                    // Expression to fetch bookings of user
                    Expression<Func<tblBooking, bool>> f2 = (p => p.cust_id == custId && p.status == true && p.seat_Number == seat && p.passCode == passCode);
                    List<tblBooking> bookings = new List<tblBooking>(bookingRepository.GetMatched(f2));

                    //Update booking status to false and Save
                    bookings.ForEach(b => b.status = false);
                    bookingRepository.Update(bookings[0]);
                }
                seatRepository.Save();
                bookingRepository.Save();

                Expression<Func<tblBooking, bool>> f3 = (p => p.cust_id == custId && p.status == true && p.passCode == passCode);
                List<tblBooking> bookingList = new List<tblBooking>(bookingRepository.GetMatched(f3));
                var seatLists = (from p in bookingList select p.seat_Number).ToList();
                List<int> existingSeatKLists = seatLists.Select(x => x.Value).ToList();

                Expression<Func<tblCustomer, bool>> f4 = (p => p.id == custId);
                List<tblCustomer> customers = new List<tblCustomer>(customerRepository.GetMatched(f4));
                var customerList = (from b in customers select new { customerName = b.FullName, customerEmail = b.Email }).FirstOrDefault();

                //Send Mail to User for confirmation
                if (objSendEmail.SendUnBookingMail(existingSeatKLists, seats, Convert.ToString(customerList.customerName), passCode, Convert.ToString(customerList.customerEmail)))
                {
                    objResponse.success = true;
                    objResponse.msg = "Cinema Seats Un-booked successfully. Please check your email-id for the un-booking details.";
                }
                else
                {
                    objResponse.success = false;
                    objResponse.msg = "Something went wrong. Error sending mail.";
                }

                //objResponse.success = true;
                //objResponse.msg = "Seats Un-booked successfully";
            }
            catch (Exception ex)
            {
                //Handling Exception and sending appropriate response to client.
                objResponse.success = false;
                objResponse.msg = "Problem Un-booking seats. Error" + ex.Message;
            }
            return Json(objResponse);
        }

        /// <summary>
        /// This method is used to check the seat availabilty
        /// </summary>
        /// <param name="seatNo"></param>
        /// <returns></returns>
        public bool CheckSeatAvailabilty(int seatNo)
        {
            tblSeat objSeat = seatRepository.GetById(seatNo);
            return objSeat.Status;
        }

        /// <summary>
        /// This method is used to check for the already registered customer
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public tblCustomer CheckForExistingCustomer(string email)
        {
            try
            {
                Expression<Func<tblCustomer, bool>> f2 = (p => p.Email == email);
                List<tblCustomer> customers = new List<tblCustomer>(customerRepository.GetMatched(f2));

                return (customers.Count > 0) ? customers.FirstOrDefault() : null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method is used to verify passcode before un-booking the booked seats.
        /// </summary>
        /// <param name="passCode"></param>
        /// <param name="custId"></param>
        /// <returns></returns>
        public bool VerifyPassCode(string passCode, int custId)
        {
            try
            {
                Expression<Func<tblBooking, bool>> f2 = (p => p.cust_id == custId && p.passCode == passCode);
                List<tblBooking> bookings = new List<tblBooking>(bookingRepository.GetMatched(f2));
                return (bookings.Count > 0) ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method is used for getting list of booking done by the user.
        /// </summary>
        /// <param name="custEmail"></param>
        /// <param name="custPassCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBooking(string custEmail, string custPassCode)
        {
            int custId = GetCustomerId(custEmail, custPassCode);
            Expression<Func<tblBooking, bool>> f2 = (p => p.cust_id == custId && p.status == true && p.passCode == custPassCode);
            List<tblBooking> bookings = new List<tblBooking>(bookingRepository.GetMatched(f2));

            return Json(new
            {
                Result = (from b in bookings select new { seatNumber = b.seat_Number, custId = b.cust_id })
            }
           , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This Method is used to get the customer id on the basis of customer-email and passcode
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="PassCode"></param>
        /// <returns></returns>
        private int GetCustomerId(string email, string passCode)
        {
            try
            {
                //Expression<Func<tblCustomer, bool>> f2 = (p => p.Email == email && p.PassCode == passCode);
                Expression<Func<tblCustomer, bool>> f2 = (p => p.Email == email);
                List<tblCustomer> customers = new List<tblCustomer>(customerRepository.GetMatched(f2));
                var chkCustomersId = (from p in customers select p.id).Single();
                return (chkCustomersId != 0) ? chkCustomersId : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}