using MovieTicketBooking.DataAccessLayer;
using MovieTicketBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Global Declaration of IRepository Instance for CRUD Operations
        /// </summary>
        private IGenericRepository<tblSeat> repository = null;

        public HomeController()
        {
            this.repository = new GenericRepository<tblSeat>();
        }

        public HomeController(IGenericRepository<tblSeat> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// This Method is used to return Home View with Seats data
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                var model = repository.GetAll();
                return View(model);
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