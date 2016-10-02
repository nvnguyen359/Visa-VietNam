using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EmailTemplate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fromEmailAddress = "nvnguyen2504@gmail.com";
            var toEmailAddress = "nvnguyen2504@gmail.com";
            var subject = "toi la ai";
          
           var template = System.IO.File.ReadAllText(HttpContext.ApplicationInstance.Server.MapPath("~/Templates/Email.cshtml"));
            var viewModel = new OrderViewModel()
            {
                SiteName = "ol",
            };
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            message.From = new System.Net.Mail.MailAddress(fromEmailAddress);
            message.To.Add(new System.Net.Mail.MailAddress(toEmailAddress));

            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = subject;
            message.Body = template;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Send(message);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}