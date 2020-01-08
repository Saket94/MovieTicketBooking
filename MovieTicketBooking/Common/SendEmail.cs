using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MovieTicketBooking
{
    public class SendEmail
    {
        /// <summary>
        /// Golbal declaration of sender email-id and password;
        /// </summary>
        string senderEmailId = Convert.ToString(ConfigurationManager.AppSettings["senderEmailId"]);
        string senderEmailPassword = Convert.ToString(ConfigurationManager.AppSettings["senderEmailPassword"]);
        int senderPort = Convert.ToInt32(ConfigurationManager.AppSettings["senderPort"]);

        /// <summary>
        /// This method is used to send email after booking is done by the customer.
        /// </summary>
        /// <param name="seatsList"></param>
        /// <param name="userName"></param>
        /// <param name="passCode"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public bool SendBookingMail(List<int> seatsList, string userName, string passCode, string userEmail)
        {
            try
            {
                using (MailMessage mail = new MailMessage(senderEmailId, userEmail))
                {
                    string seatNumbers = String.Join(",", seatsList);
                    string strMsg = "";
                    strMsg = " <table style=\"border-collapse:collapse; text-align:center;\" border=\"1\">" +
                                "<tr><td><b>SrNo</b></td><td><b>Booked Seat Number</b></td></tr>";
                    int seqNo = 1;
                    foreach (var item in seatsList)
                    {
                        strMsg = strMsg + " <tr><td> " + seqNo + "</td><td>" + item + "</td>" + "</td></tr>";
                        seqNo++;
                    }
                    strMsg = strMsg + "</table>";
                    mail.Subject = "Ticket Booking Confirmation";
                    mail.Body = "Hello " + userName + ",";
                    mail.Body += "<br /><br />You had successfully booked Cinema tickets";
                    mail.Body += "<br /><br />Your passcode for this transaction is:-" + "<b>" + passCode + "</b>";
                    mail.Body += "<br /><br />Please find your seat details below:-";
                    mail.Body += "<br /><br />" + strMsg;
                    mail.Body += "<br />Thanks and Regards";
                    mail.Body += "<br />Cinema Ticket Booking";
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(senderEmailId, senderEmailPassword);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = senderPort;
                        smtp.Send(mail);
                        //ViewBag.Message = "Email sent.";
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This Method is used to send email when customer un-booked the seats.
        /// </summary>
        /// <param name="existingSeatKLists"></param>
        /// <param name="unbookedSeatLists"></param>
        /// <param name="userName"></param>
        /// <param name="passCode"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public bool SendUnBookingMail(List<int> existingSeatKLists, List<int> unbookedSeatLists, string userName, string passCode, string userEmail)
        {
            try
            {
                using (MailMessage mail = new MailMessage(senderEmailId, userEmail))
                {
                    string seatNumbers = String.Join(",", existingSeatKLists);
                    string strExistingSeatsMsg = ""; string strUnBookedSeatsMsg = "";
                    string strExistingSeatsList = ""; string strUnBookedSeatsList = "";
                    int existingSeqNo = 1; int unBookedSeqNo = 1;
                    if (existingSeatKLists.Count() > 0)
                    {
                        strExistingSeatsList = " <table style=\"border-collapse:collapse; text-align:center;\" border=\"1\">" +
                                "<tr><td><b>SrNo</b></td><td><b>Booked Seat Number</b></td></tr>";
                        foreach (var item in existingSeatKLists)
                        {
                            strExistingSeatsList = strExistingSeatsList + " <tr><td> " + existingSeqNo + "</td><td>" + item + "</td>" + "</td></tr>";
                            existingSeqNo++;
                        }
                        strExistingSeatsList = strExistingSeatsList + "</table>";
                        strExistingSeatsMsg = "Please find your booked seat details below:-";
                    }

                    if (unbookedSeatLists.Count() > 0)
                    {
                        strUnBookedSeatsList = " <table style=\"border-collapse:collapse; text-align:center;\" border=\"1\">" +
                                "<tr><td><b>SrNo</b></td><td><b>Un-Booked Seat Number</b></td></tr>";
                        foreach (var item in unbookedSeatLists)
                        {
                            strUnBookedSeatsList = strUnBookedSeatsList + " <tr><td> " + unBookedSeqNo + "</td><td>" + item + "</td>" + "</td></tr>";
                            unBookedSeqNo++;
                        }
                        strUnBookedSeatsList = strUnBookedSeatsList + "</table>";
                        strUnBookedSeatsMsg = "Please find your un-booked seat details below:-";
                    }


                    mail.Subject = "Ticket Un-Booking Confirmation";
                    mail.Body = "Hello " + userName + ",";
                    mail.Body += "<br /><br />You had successfully booked Cinema tickets";
                    mail.Body += "<br /><br />Your passcode for this transaction is:-" + "<b>" + passCode + "</b>";
                    if (existingSeatKLists.Count() > 0)
                    {
                        mail.Body += "<br /><br />" + strExistingSeatsMsg;
                        mail.Body += "<br />" + strExistingSeatsList;
                    }
                    if (unbookedSeatLists.Count() > 0)
                    {
                        mail.Body += "<br /><br />" + strUnBookedSeatsMsg;
                        mail.Body += "<br />" + strUnBookedSeatsList;
                    }
                    mail.Body += "<br />Thanks and Regards";
                    mail.Body += "<br />Cinema Ticket Booking";
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(senderEmailId, senderEmailPassword);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = senderPort;
                        smtp.Send(mail);
                        //ViewBag.Message = "Email sent.";
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}