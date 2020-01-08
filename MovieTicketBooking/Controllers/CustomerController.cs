using MovieTicketBooking.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieTicketBooking.Models;
using System.Linq.Expressions;
using MovieTicketBooking.Helpers;

namespace MovieTicketBooking.Controllers
{
    public class CustomerController : Controller
    {
        /// <summary>
        /// Global Declaration of IRepository Instance for CRUD Operations
        /// </summary>
        private IGenericRepository<tblCustomer> repository = null;

        public CustomerController()
        {
            this.repository = new GenericRepository<tblCustomer>();
        }

        public CustomerController(IGenericRepository<tblCustomer> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// This method is used to get Costomer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCustomer(int id)
        {
            try
            {
                tblCustomer customer = repository.GetById(id);
                return Json(customer);
            }
            catch (Exception ex)
            {
                Helpers.Response response = new Helpers.Response();
                response.success = false;
                response.msg = "Error :" + ex.Message;
                return Json(response);
            }
        }

        /// <summary>
        /// This method is used to get Costomer Email by character matching
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult GetCustomerEmail(string param)
        {
            try
            {
                //Create expression to fetch costumer by character matching
                Expression<Func<tblCustomer, bool>> f2 = (p => p.Email.Contains(param));
                List<tblCustomer> customers = new List<tblCustomer>(repository.GetMatched(f2));

                //Declare object for AutoSuggest list and fill and send to View
                List<AutoSuggest> values = new List<AutoSuggest>();
                values = customers.Select(C => new AutoSuggest
                {
                    id = C.id,
                    text = C.Email
                }).ToList();

                return (Json(new { items = values }, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Helpers.Response response = new Helpers.Response();
                response.success = false;
                response.msg = "Error :" + ex.Message;
                return Json(response);
            }
        }
    }
}