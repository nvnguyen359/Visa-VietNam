using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Visa.Controllers
{
    public class DefaultController : Controller
    {
        VisaDataContext myData = new VisaDataContext();
        static int counter = 0;
        object lockObj = new object();

        //private int i = 0;
        // GET: Default
        public ActionResult Index(int? id)
        {
            //string smtpUserName = "nvnguyen2504@gmail.com";
            //string smtpPassword = "mothaiba123";
            //string smtpHost = "smtp.gmail.com";
            //int smtpPort = 25;
            //string ret = RenderRazorViewToString("~/Templates/Email.cshtml", "");
            //string emailTo = "nvnguyen2504@gmail.com"; // Khi có liên hệ sẽ gửi về thư của mình
            //string subject = "oooo";
            //EmailService service = new EmailService();

            //bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort,
            //    emailTo, subject, ret);
            ViewBag.note = myData.NOTEs.ToList();
            Session["private"] = 0;
          
            var k1 = myData.ViSaPart1s.Where(j => j.typeOfVisa != null && j.pax1 == null);
            if (k1 != null)
            {
                myData.ViSaPart1s.DeleteAllOnSubmit(k1);
                myData.SubmitChanges();
            }
            var firstOrDefault = myData.Processingtimes.FirstOrDefault(k => k.Id.Equals(1));
            if (firstOrDefault != null)
                ViewBag.thongtintime = firstOrDefault.ghichu;
            ViewBag.fromdate = myData.NOTEs.FirstOrDefault(k => k.id.Equals(2)).note1;
            ViewBag.exitdate = myData.NOTEs.FirstOrDefault(k => k.id.Equals(3)).note1;
            ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
            ViewBag.qt = myData.Nationalities.ToList();
            ViewBag.Purpose = myData.TPurposeofvisias.ToList();

            ViewBag.Port = myData.TArrivalPorts.ToList();
            ViewBag.processtime = myData.Processingtimes.ToList();
            ViewBag.dem = id;
            ViewBag.typeOfVisa =
                myData.ViSaPart1s.ToList().FindAll(j => j.pax1 > 0 && j.visit.ToLower().Contains("tourist")).ToList();
            if (Session["sothanhvien"] == null)
            {
                Session["sothanhvien"] = "0";
            }
            else
            {
                ViewBag.songuoi = Session["sothanhvien"];
            }
            return View();
        }

        public class EmailService
        {
            /// <summary>
            /// Hàm thực thi gửi email. 
            /// </summary>
            /// <param name="smtpUserName">Tên đăng nhập email gửi thư: vd:tuanitpro</param>
            /// <param name="smtpPassword">Mật khẩu của email gửi thư</param>
            /// <param name="smtpHost">Host email. vd smtp.gmail.com</param>
            /// <param name="smtpPort">Port vd: 465</param>
            /// <param name="toEmail">Email nhận vd: tuanitpro@gmail.com</param>
            /// <param name="subject">Chủ đề</param>
            /// <param name="body">Nội dung thư gửi</param>
            /// <returns>True-Thành công/False-Thất bại</returns>
            public bool Send(string smtpUserName, string smtpPassword, string smtpHost, int smtpPort,
                string toEmail, string subject, string body)
            {
                try
                {
                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.Host = smtpHost;
                        smtpClient.Port = smtpPort;
                        smtpClient.UseDefaultCredentials = true;
                        smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                        var msg = new MailMessage
                        {
                            IsBodyHtml = true,
                            BodyEncoding = Encoding.UTF8,
                            From = new MailAddress(smtpUserName),
                            Subject = subject,
                            Body = body,
                            Priority = MailPriority.Normal,
                        };

                        msg.To.Add(toEmail);

                        smtpClient.Send(msg);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        [HttpPost]
        public ActionResult Index(string id = "1")
        {


            ViewBag.note = myData.NOTEs.ToList();
            ViewBag.dschongoi = myData.CARPICKUPs.Select(j => j.choNgoi).Distinct();
            ViewBag.typeOfVisa =
                myData.ViSaPart1s.ToList().FindAll(j => j.pax1 > 0 && j.visit.ToLower().Contains("tourist")).ToList();
            var firstOrDefault = myData.Processingtimes.FirstOrDefault();
            if (firstOrDefault != null)
                ViewBag.thongtintime = firstOrDefault.ghichu;
            ViewBag.dem = id;
            if (Session["sothanhvien"] == null)
            {
                Session["sothanhvien"] = "0";
            }
            else
            {
                ViewBag.songuoi = Session["sothanhvien"];
            }
            return View();
        }

        public ActionResult ChangeArrivaldate(string date)
        {
            try
            {
                if (date != "")
                {
                    var dateh = DateTime.Parse(date);
                    if (dateh == DateTime.Today)
                    {
                        if (DateTime.UtcNow.AddHours(7).Hour >= 12)
                        {
                            var tb = myData.NOTEs.FirstOrDefault(j => j.id.Equals(7));
                            return Json(tb.note1 + "*" + (DateTime.UtcNow.AddHours(7).Hour - 12),
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("ok", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {


            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaxExitdate(string dt, string thag)
        {
            try
            {
                ViewBag.note = myData.NOTEs.ToList();
                var tg = dt.Split('-');
                var thang = myData.TTypeofvisas.FirstOrDefault(j => j.name.Contains(thag)).value;
                var t =
                    new DateTime(Shared.ParseInt(tg[0]), Shared.ParseInt(tg[1]), Shared.ParseInt(tg[2])).AddMonths(
                        (int) thang);
                return Json(t.Year + "-" + Shared.Add0(t.Month) + "-" + Shared.Add0(t.Day), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return
                    Json(
                        DateTime.Today.Year + "-" + Shared.Add0(DateTime.Today.Month) + "-" +
                        Shared.Add0(DateTime.Today.Day), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Visa11()
        {
            ViewBag.note = myData.NOTEs.ToList();
            ViewBag.Purpose = myData.ViSaPart1s.Where(j => j.visit.Contains("TOURIST VISA") && j.pax1 > 0).ToList();
            ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
            ViewBag.tablePrice1 = myData.ViSaPart1s.ToList();
            //ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            ViewBag.ter = "toi la ai " + Session["visa11"];
            return View();
        }

        [HttpPost]
        public ActionResult Visa11(ViSaPart1 visa1)
        {
            ViewBag.note = myData.NOTEs.ToList();
            if (ModelState.IsValid && visa1 != null)
            {
                var kt = myData.ViSaPart1s.FirstOrDefault(j => j.typeOfVisa.Equals(visa1.typeOfVisa)
                                                               && j.pax1.Equals(visa1.pax1) && j.pax2.Equals(visa1.pax2) &&
                                                               j.pax3.Equals(visa1.pax3)
                                                               && j.pax4.Equals(visa1.pax4));
                if (kt != null)
                {
                    myData.ViSaPart1s.InsertOnSubmit(visa1);
                    myData.SubmitChanges();
                }

            }

            ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            return View();
        }

        public ActionResult ItemTruongdoan()
        {
            ViewBag.note = myData.NOTEs;
            try
            {
                var kt =
                    myData.TRUONDOANs.FirstOrDefault(
                        j => j.id.Equals(Shared.Parselong(Session["idtruongdoan"].ToString())));
                if (kt != null)
                {
                    return Json(kt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {


            }
            return Json("Khong co du lieu", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateVisa11(int id, string name, int pax1, int pax2, int pax3, int pax4, string visit,
            int stamp)
        {
            ViewBag.note = myData.NOTEs;
            var tb = "tb";
            if (ModelState.IsValid)
            {
                var kt = myData.ViSaPart1s.FirstOrDefault(j => j.id.Equals(id));
                if (kt != null)
                {
                    kt.typeOfVisa = name;
                    kt.pax1 = pax1;
                    kt.pax2 = pax2;
                    kt.pax3 = pax3;
                    kt.pax4 = pax4;
                    kt.visit = visit;
                    //kt.proTime = visit1;
                    kt.valueP = stamp;
                    myData.SubmitChanges();
                    tb = "Thanh cong";
                }

                ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
                ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);

            }
            return Json(tb, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPriceVisa()
        {
            ViewBag.note = myData.NOTEs;
            ViewBag.Tourist = myData.ViSaPart1s.Where(j => j.visit.Contains("TOURIST")).ToList();
            ViewBag.Busines = myData.ViSaPart1s.Where(j => j.visit.Contains("BUSINESS")).ToList();
            ViewBag.feeT =
                myData.Processingtimes.Where(j => j._Purposeofvisit.Equals("Tourist") && j.name != "Normal ").ToList();
            ViewBag.feeB =
                myData.Processingtimes.Where(j => j._Purposeofvisit.Equals("Business") && j.name != "Normal ").ToList();
            return View();
        }

        public ActionResult AddVisa11(int t1, int t2, int t3, int t4, string t, string visit, int stamp)
        {
            var tb = "tb";
            if (ModelState.IsValid)
            {
                var kt = myData.ViSaPart1s.FirstOrDefault(j => j.typeOfVisa.Equals(t) && j.visit.Equals(visit));
                if (kt != null)
                {
                    kt.typeOfVisa = t;
                    kt.pax1 = t1;
                    kt.pax2 = t2;
                    kt.pax3 = t3;
                    kt.pax4 = t4;
                    kt.visit = visit;
                    kt.valueP = stamp;
                    myData.SubmitChanges();
                    tb = "Thanh cong";

                }
                else
                {
                    myData.ViSaPart1s.InsertOnSubmit(new ViSaPart1()
                    {
                        typeOfVisa = t,
                        pax1 = t1,
                        pax2 = t2,
                        pax3 = t3,
                        pax4 = t4,
                        visit = visit,
                        valueP = stamp
                    });
                    myData.SubmitChanges();
                }
                ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
                ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
                ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
                ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);
            }
            return Json(tb, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Visa1()
        {
            ViewBag.note = myData.NOTEs;
            ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
            ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);
            return View();
        }

        [HttpPost]
        public ActionResult Visa1(BGIAVISA1 visa1)
        {
            ViewBag.note = myData.NOTEs;
            var stamp = Request.Form["stamp"];
            if (ModelState.IsValid && visa1 != null)
            {
                myData.BGIAVISA1s.InsertOnSubmit(visa1);
                myData.SubmitChanges();
            }
            ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
            ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            return View();
        }

        public ActionResult Visa21()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Visa2(BGIAVISA2 visa1)
        {
            ViewBag.note = myData.NOTEs;
            if (ModelState.IsValid)
            {
                myData.BGIAVISA2s.InsertOnSubmit(visa1);
                myData.SubmitChanges();
            }
            return View();
        }

        public ActionResult Visa3()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Visa3(BGIAVISA3 visa1)
        {
            if (ModelState.IsValid)
            {
                myData.BGIAVISA3s.InsertOnSubmit(visa1);
                myData.SubmitChanges();
            }
            return View();
        }

        public ActionResult QuocTichDacBiet()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult QuocTichDacBiet(QuocGiaDacBiet qt)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        myData.QuocGiaDacBiets.InsertOnSubmit(qt);
        //        myData.SubmitChanges();
        //    }
        //    ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
        //    ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
        //    return View();
        //}

        public ActionResult VisaOptions()
        {

            ViewBag.note = myData.NOTEs.ToList();
            ViewDataSteps2();
            DsSteps2();
            return View();
        }

        public void ViewDataSteps2()
        {
            ViewBag.note = myData.NOTEs.ToList();
            //var firstOrDefault = myData.Processingtimes.FirstOrDefault(k => k.Id.Equals(1));
            //if (firstOrDefault != null)
            //    ViewBag.thongtintime = firstOrDefault.ghichu;
            //ViewBag.fromdate = myData.NOTEs.FirstOrDefault(k => k.id.Equals(2)).note1;
            //ViewBag.exitdate = myData.NOTEs.FirstOrDefault(k => k.id.Equals(3)).note1;
            ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
            ViewBag.qt = myData.Nationalities.ToList();
            ViewBag.Purpose = myData.TPurposeofvisias.ToList();
            ViewBag.note = myData.NOTEs.ToList();
            ViewBag.Port = myData.TArrivalPorts.ToList();
            ViewBag.processtime = myData.Processingtimes.ToList();

            ViewBag.typeOfVisa =
                myData.ViSaPart1s.ToList().FindAll(j => j.visit.ToLower().Contains("tourist") && j.pax1 > 0).ToList();
        }

        public string RenderRazorViewToString(string viewName, object model)
        {

            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult TimeUtc(string d)
        {
            if (d != "")
            {
                var time = DateTime.Parse(d);
            }
            return Json(DateTime.UtcNow.AddHours(7) + " Viet Nam");

        }

        [HttpPost]
        public ActionResult VisaOptions(TRUONDOAN tr)
        {
            Session["sothanhvien"] = 0;
            //Session["tienTv"] = 0;
            //Session["tienPlus"] = 0;
            //Session["private"] = 0;
            try
            {
                ViewBag.note = myData.NOTEs.ToList();
                var ktemail = myData.TRUONDOANs.FirstOrDefault(j => j.email.Equals(tr.email));

                if (ModelState.IsValid)
                {
                    if (ktemail == null)
                    {
                        tr.ngay = DateTime.Today;
                        tr.payMember = Shared.ParseInt(Session["tienTv"].ToString());
                        tr.valueProTime = Shared.ParseDouble(Session["tienPlus"].ToString());
                        tr.total = Shared.ParseInt(Session["tiendv"].ToString()) +
                                   Shared.ParseInt(Session["private"].ToString());
                        if(Session["private"]!=null)
                        tr.valuePrivate = Shared.ParseInt(Session["private"].ToString());
                        myData.TRUONDOANs.InsertOnSubmit(tr);
                        myData.SubmitChanges();
                        Session["sothanhvien"] = tr.numberofapplicant;

                        ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
                        var firstOrDefault =
                            myData.TRUONDOANs.FirstOrDefault(
                                j => j.email.Equals(tr.email) && j.ngay.Equals(DateTime.Today));
                        if (firstOrDefault != null)
                        {
                            Session["idtruongdoan"] = firstOrDefault.id.ToString();
                            tr.invoice = Shared.InvoiceNumberVisa("VS", (int) firstOrDefault.id);
                            myData.SubmitChanges();
                        }
                        ViewDataSteps2();
                        DsSteps2();
                        return View("Steps2s");
                    }
                    else
                    {
                        ViewDataSteps2();
                        DsSteps2();
                        ViewBag.step1 = " Email da co trong tai khoan";
                        return Redirect("Index");
                    }



                }

            }
            catch (Exception)
            {
                ViewDataSteps2();
                DsSteps2();
                ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
                return View("Index");
            }
            ViewDataSteps2();
            DsSteps2();
            return View();
        }

        public ActionResult EditFastTracks(string kt, string value)
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    sb.valueFast = Shared.ParseDouble(value.Split(' ')[0]);
                    sb.fastTrack = kt;
                    myData.SubmitChanges();
                    return Json(kt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {


            }
            return Json("");
        }

        public ActionResult SumReviewStep3()
        {
            Session["tongty"] = 0;
            Session["tongty1"] = 0;
            var sum = 0.0;
            var id = long.Parse(Session["idtruongdoan"].ToString());
            var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
            if (sb != null)
            {
                List<string> s = new List<string>();
                s.Add("Invoice number: " + sb.invoice);
                s.Add("Total applicants: " + sb.numberofapplicant);
                var sumpay1 = myData.THANHVIENs.Where(k => k.idTrDoan.Equals(id)).Sum(j => j.pay1);
                s.Add(sb.typeofvisa + ": " +sumpay1);
                sum += Shared.ParseDouble(sumpay1.ToString());
                Session["tongty"] = Shared.ParseDouble(sumpay1.ToString());
                var sumpay2 = myData.THANHVIENs.Where(k => k.idTrDoan.Equals(id)).Sum(j => j.pay2);
                s.Add(sb.processingtime + ": " + sumpay2);
                sum += Shared.ParseDouble(sumpay2.ToString());
                if (sb.valueFast > 0)
                {
                    s.Add("Fast - Track:" + sb.valueFast);
                    sum += (double) sb.valueFast;
                }
                if (sb.valueCar > 0)
                {
                    s.Add("Car:" + sb.valueCar);
                    sum += (double) sb.valueCar;
                }
                if (sb.valuePrivate > 0)
                {
                    s.Add("Private:" + sb.valuePrivate);
                    sum += (double) sb.valuePrivate;
                    Session["tongty"] = sum;
                }
                s.Add("Sum:    " + sum + "USD");
                Session["tongty1"] = sum - Shared.ParseDouble(Session["tongty"].ToString());
                return Json(s, JsonRequestBehavior.AllowGet);

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SumApp(string apply)
        {
            var kt =
                myData.APPLYCODEs.FirstOrDefault(
                    j =>
                        j.code.Equals(apply.ToLower()) && j.tu <= DateTime.UtcNow.AddHours(7) &&
                        DateTime.UtcNow.AddHours(7) <= j.den);
            var cde = 0.0;

            var tong = Shared.ParseDouble(Session["tongty"].ToString());
            if (kt != null)
            {
                if (kt.value != null || kt.value > 0)
                {
                    cde = tong - Shared.ParseDouble(kt.value.ToString());
                }
                else
                {
                    if (kt.valuePercent != null || kt.valuePercent > 0)
                    {
                        cde = tong - Shared.ParseDouble(kt.value.ToString())*tong;
                    }
                }
                return Json(cde + Shared.ParseDouble(Session["tongty1"].ToString()), JsonRequestBehavior.AllowGet);
            }
            return Json("Error code", JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditCar(string kt, string value)
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    sb.carPick = kt;
                    sb.valueCar = Shared.ParseDouble(value.Split(' ')[0]);
                    myData.SubmitChanges();
                    return Json(kt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {


            }
            return Json("");
        }

        public ActionResult EditHotel(string kt)
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    sb.hotel = kt;
                    myData.SubmitChanges();
                    return Json(kt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {


            }
            return Json("");
        }

        public ActionResult ViewFastTrack()
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    var kt =
                        myData.AIRPORTFASTRACKs.ToList()
                            .FindAll(
                                j =>
                                    (Shared.ParseInt(j.soGhe.Split(' ')[0]) <=
                                     Shared.ParseInt(sb.numberofapplicant) &&
                                     Shared.ParseInt(j.soGhe.Split(' ')[2]) >=
                                     Shared.ParseInt(sb.numberofapplicant)) && j.sanBay.Contains(sb.arrivalPort));
                    return Json(kt, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {


            }
            return Json("");
        }

        public ActionResult Steps1()
        {
            return View();
        }

        public ActionResult ThanhToanSumPay()
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                return Json(sb, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {


            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThanhToanFastTrack(string name)
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    var kt =
                        myData.AIRPORTFASTRACKs.ToList()
                            .Find(
                                j =>
                                    (Shared.ParseInt(j.soGhe.Split(' ')[0]) <=
                                     Shared.ParseInt(sb.numberofapplicant) &&
                                     Shared.ParseInt(j.soGhe.Split(' ')[2]) >=
                                     Shared.ParseInt(sb.numberofapplicant)) && j.sanBay.Contains(sb.arrivalPort)
                                    && j.air.Contains(name));
                    if (kt != null)
                    {
                        return Json(kt.donGia1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GiaTienCho(string socho)
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sb = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id) && j.ngay.Equals(DateTime.Today));
                if (sb != null)
                {
                    var kt =
                        myData.CARPICKUPs.FirstOrDefault(
                            j => j.sanBay.Contains(sb.arrivalPort) && j.choNgoi.Contains(socho));
                    if (kt != null)
                    {
                        return Json(kt.donGia + " " + kt.dvTien, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowChongoi()
        {
            var kt = myData.CARPICKUPs.Select(j => j.choNgoi).Distinct();
            return Json(kt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KiemtraBStep2()
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var sotv = myData.THANHVIENs.Count(j => j.idTrDoan.Equals(id));
                var tongtv = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id));
                var kt = (Shared.ParseInt(tongtv.numberofapplicant.ToString()) - sotv);
                Session["kiemtra"] = kt;
                return Json(kt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {


            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public void DsSteps2()
        {
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                ViewBag.dataThanhvien = myData.THANHVIENs.Where(j => j.idTrDoan.Equals(id));
                ViewBag.tr = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id));
                ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
                ViewBag.note = myData.NOTEs.ToList();
                ViewBag.sokhachj = myData.THANHVIENs.Count(j => j.idTrDoan.Equals(id)) + 1;
                ViewBag.dschongoi = myData.CARPICKUPs.Select(j => j.choNgoi).Distinct();
                ViewBag.dssabay = myData.CARPICKUPs.Select(j => j.sanBay).Distinct();
            }
            catch (Exception)
            {


            }

        }

        public ActionResult Steps2()
        {
            ViewBag.gender = myData.Genders.ToList();
            ViewBag.songuoi = Session["sothanhvien"];
            return View();
        }

        public ActionResult LoadGender()
        {
            return Json(myData.Genders.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Loadquoctich()
        {
            return Json(myData.Nationalities.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Steps2s(int? dem = 0)
        {
            Random r = new Random();
            ViewBag.note = myData.NOTEs.ToList();
            //Session["sothanhvien"] = dem.ToString();
            ViewBag.idtruongdoan = Session["idtruongdoan"];
            ViewBag.songuoi = Session["sothanhvien"];
            var id = long.Parse(Session["idtruongdoan"].ToString());
            DsSteps2();
            ViewDataSteps2();
            return View();
        }

        public ActionResult EditThanhVien()
        {
            return View("Steps2s");
        }

        [HttpPost]
        public ActionResult AddThanhVien(THANHVIEN tv)
        {
            DsSteps2();
            ViewDataSteps2();
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var trdoan = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id));
                var kt = Shared.ParseInt(Session["kiemtra"].ToString());

                if (ModelState.IsValid)
                {

                    if (tv.id.Equals(0) && tv.fullName != null)
                    {
                        if (kt > 0)
                        {

                            tv.idTrDoan = long.Parse(Session["idtruongdoan"].ToString());
                            tv.ngay = DateTime.Today;
                            if (trdoan != null)
                            {
                                if (Shared.ParseInt(Session["specalDb"].ToString()) > 0)
                                {
                                    tv.pay1 = Shared.ParseInt(Session["specalDb"].ToString());
                                    tv.pay2 = Shared.ParseInt(trdoan.valueProTime.ToString());
                                    myData.THANHVIENs.InsertOnSubmit(tv);
                                    myData.SubmitChanges();
                                    ViewBag.sokhachj = myData.THANHVIENs.Count(j => j.idTrDoan.Equals(id)) + 1;
                                }
                                if (Shared.ParseInt(Session["specalDb"].ToString()) ==-1 )
                                {
                                    tv.pay1 = Shared.ParseInt(trdoan.payMember.ToString());
                                    tv.pay2 = Shared.ParseInt(trdoan.valueProTime.ToString());
                                    myData.THANHVIENs.InsertOnSubmit(tv);
                                    myData.SubmitChanges();
                                    ViewBag.sokhachj = myData.THANHVIENs.Count(j => j.idTrDoan.Equals(id)) + 1;
                                }
                              
                              
                            }

                          

                            return View("Steps2s");

                        }
                        //if (sokhach >= int.Parse(Session["sothanhvien"].ToString()))
                        //{
                        //    return View("Steps3");
                        //}

                    }
                    else
                    {
                        return View("Steps2s");
                    }


                }
            }
            catch (Exception)
            {


            }
            return View("Index");

        }

        [HttpPost]
        public ActionResult UpdateThanhVien(long idtv, string name, string gender, string sinhnhat, string sopass,
            string hethan)
        {

            var kt = myData.THANHVIENs.FirstOrDefault(j => j.id.Equals(idtv));
            if (kt != null)
            {
                kt.fullName = name;
                kt.gender = gender;
                kt.pateOfBirth = DateTime.Parse(sinhnhat);
                kt.passPortNumber = sopass;
                kt.passportexpireddate = DateTime.Parse(hethan);
                myData.SubmitChanges();

            }
            return Json("ok", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult EditThanhVien(long idtv)
        {
            var hg = myData.THANHVIENs.FirstOrDefault(k => k.id.Equals(idtv));
            if (hg != null)
            {
                myData.THANHVIENs.DeleteOnSubmit(hg);
                myData.SubmitChanges();
                return Json("Thanh Cong", JsonRequestBehavior.AllowGet);

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowThanhVien(long idtv)
        {
            var hg = myData.THANHVIENs.FirstOrDefault(k => k.id.Equals(idtv));
            if (hg != null)
            {
                return Json(hg, JsonRequestBehavior.AllowGet);

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddThanhVien()
        {
            ViewDataSteps2();
            DsSteps2();

            return View("Steps2s");
        }

        public ActionResult DuidataSteps2(string a)
        {
            if (Session["thongtin"] == null)
            {
                Session["thongtin"] = 0;
            }
            else
            {
                ViewBag.ttin = Session["thongtin"];
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Steps2s()
        {
            var id = long.Parse(0.ToString());
            //Random r = new Random();
            //Session["sothanhvien"] = r.Next(50);
            //if (Session["sothanhvien"] == null)
            //{
            //    Session["sothanhvien"] = "0";
            //}
            //else
            //{
            //    ViewBag.songuoi = Session["sothanhvien"];
            //}
            if (Session["idtruongdoan"] == null)
            {
                Session["idtruongdoan"] = 0;
            }
            else
            {
                ViewBag.idTruongdoan = Session["idtruongdoan"];
                id = Shared.Parselong(Session["idtruongdoan"].ToString());
            }


            ViewBag.dataThanhvien = myData.THANHVIENs.Where(j => j.idTrDoan.Equals(id));
            ViewBag.tr = myData.TRUONDOANs.Where(j => j.id.Equals(id));
            //var id = long.Parse(Session["idtruongdoan"].ToString());
            var sokhach = myData.THANHVIENs.Count(j => j.id.Equals(id));

            DsSteps2();
            ViewDataSteps2();
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachSteps2s()
        {
            ViewDataSteps2();
            DsSteps2();
            return View();
        }

        [HttpPost]
        public ActionResult DanhSachSteps2s(int f)
        {
            //var id = long.Parse(Session["idtruongdoan"].ToString());
            DsSteps2();
            ViewDataSteps2();
            return View();
        }

        public ActionResult Steps3()
        {
            ViewBag.dschongoi = myData.CARPICKUPs.Select(j => j.choNgoi).Distinct();
            DsSteps2();
            ViewDataSteps2();
            return View();
        }

        public ActionResult Nhap()
        {

            return View();
        }

        public ActionResult Step1()
        {
            return View();
        }

        private bool kiemtralogin = false;

        public ActionResult Quanly()
        {

            try
            {

                var username = userInfo["name"];
                var matkhau = userInfo["pass"];
                //ViewBag.taikhoan =""
                var kt = myData.TAIKHOANs.FirstOrDefault(r => (r.name.Equals(userInfo["name"])
                                                               ||
                                                               r.email.Equals(userInfo["name"]) &&
                                                               r.pass.Equals(userInfo["pass"])));

                if (kt != null)
                {
                    if (kt.auto.Equals(true))
                    {
                        return Admin();
                    }

                }

            }
            finally
            {

            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult Quanly(TAIKHOAN t, FormCollection cl, string ah)
        {
            Login(cl, ah);
            if (kiemtralogin == true)
            {
                DanhSachAdmin();
                //ViewBag.list = myData.Nationalities.ToList().Count;
                return View("Admin");
            }

            return View();
        }

        public ActionResult PostNationalities(string quoctich)
        {
            var qt = new Nationality() {Name = quoctich};
            myData.Nationalities.InsertOnSubmit(qt);
            myData.SubmitChanges();
            ViewBag.quoctich = myData.Nationalities.ToList().OrderByDescending(r => r.id);
            return View();
        }

        [HttpPost]
        public int PostNationalities(List<string> n, List<string> value1, string quoctich)
        {

            //for (int i = 0; i < n.Count; i++)
            //{
            //    var qt1 = new Nationality() { Name = n[i],value = value1[i], stt = 1 };
            //       myData.Nationalities.InsertOnSubmit(qt1);
            //}

            if (quoctich != "" && myData.Nationalities.FirstOrDefault(f => f.Name.Equals(quoctich)) != null)
            {
                var qt1 = new Nationality() {Name = quoctich, stt = 1};
                myData.Nationalities.InsertOnSubmit(qt1);
            }

            myData.SubmitChanges();
            ViewBag.quoctich = myData.Nationalities.DistinctBy(j => j.Name).OrderByDescending(r => r.id).ToList();
            return myData.Nationalities.ToList().Count;
        }

        [HttpPost]
        public int QuocTich(string quoctich, string valueff)
        {

            //for (int i = 0; i < n.Count; i++)
            //{
            //    var qt1 = new Nationality() { Name = n[i],value = value1[i], stt = 1 };
            //       myData.Nationalities.InsertOnSubmit(qt1);
            //}

            if (quoctich != "" && myData.Nationalities.FirstOrDefault(f => f.Name.Equals(quoctich)) == null)
            {
                var qt1 = new Nationality() {Name = quoctich, stt = 1, value = valueff};
                myData.Nationalities.InsertOnSubmit(qt1);
            }

            myData.SubmitChanges();
            ViewBag.quoctich = myData.Nationalities.DistinctBy(j => j.Name).OrderByDescending(r => r.id).ToList();
            return myData.Nationalities.ToList().Count;
        }

        public int DeleteQt(string quoctich)
        {
            var qt = myData.Nationalities.FirstOrDefault(f => f.Name.Equals(quoctich));

            if (quoctich != "" && qt != null)
            {

                myData.Nationalities.DeleteOnSubmit(qt);
            }

            myData.SubmitChanges();
            ViewBag.quoctich = myData.Nationalities.DistinctBy(j => j.Name).OrderByDescending(r => r.id).ToList();
            return myData.Nationalities.ToList().Count;
        }

        public void DanhSachAdmin()
        {
            ViewBag.thongbao = myData.TAIKHOANs.Where(r => r.id > 0).ToList().Count;
            ViewBag.admin = myData.TAIKHOANs.Where(r => r.id > 0).ToList();
            ViewBag.quoctich = myData.Nationalities.DistinctBy(j => j.Name).OrderByDescending(r => r.id).ToList();
            ViewBag.sanbay = myData.TArrivalPorts.ToList();
            ViewBag.sex = myData.Genders.ToList();
            ViewBag.giaphong = myData.TGiaPhongs.ToList();
            ViewBag.diadiem = myData.TLocaltions.ToList();
            ViewBag.rom = myData.TRomtypes.ToList();
            ViewBag.numberofvisa = myData.TNumberofapplicants.ToList();
        }

        public int SanBay(string quoctich)
        {


            if (quoctich != "" && myData.TArrivalPorts.FirstOrDefault(f => f.name.Equals(quoctich)) == null)
            {
                var qt1 = new TArrivalPort() {name = quoctich};
                myData.TArrivalPorts.InsertOnSubmit(qt1);
            }

            myData.SubmitChanges();
            ViewBag.sanbay = myData.TArrivalPorts.ToList();
            return myData.TArrivalPorts.ToList().Count;
        }

        public int GioiTinh(string sex)
        {


            if (sex != "" && myData.Genders.FirstOrDefault(f => f.Name.Equals(sex)) == null)
            {
                //var qt1 = new Gender() { Name = sex };
                myData.Genders.InsertOnSubmit(new Gender() {Name = sex});
                myData.SubmitChanges();
            }


            ViewBag.sex = myData.Genders.ToList();
            return myData.Genders.ToList().Count;
        }

        public int DeleteSanbay(string quoctich)
        {
            var qt = myData.TArrivalPorts.FirstOrDefault(f => f.name.Equals(quoctich));

            if (quoctich != "" && qt != null)
            {

                myData.TArrivalPorts.DeleteOnSubmit(qt);
            }

            myData.SubmitChanges();
            ViewBag.sanbay = myData.TArrivalPorts.ToList();
            return myData.TArrivalPorts.ToList().Count;
        }

        public int DeleteSex(string sex)
        {
            var qt = myData.Genders.FirstOrDefault(f => f.Name.Equals(sex));

            if (sex != "" && qt != null)
            {

                myData.Genders.DeleteOnSubmit(qt);
            }

            myData.SubmitChanges();
            ViewBag.sex = myData.Genders.ToList();
            return myData.Genders.ToList().Count;
        }

        public ActionResult Admin()
        {
            DanhSachAdmin();
            return View();
        }

        HttpCookie userInfo = new HttpCookie("taikhoan");

        public ActionResult Login()
        {
            ViewBag.thongbao = "Get";
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection cl, string ah)
        {
            ViewBag.thongbao = "Post";
            if (ModelState.IsValid)
            {
                var matkhau = myClass.MahoaMd5(cl["pass"]);
                var account =
                    myData.TAIKHOANs.FirstOrDefault(
                        r => r.name.Equals(cl["name"]) || r.email.Equals(cl["name"]) && r.pass.Equals(matkhau));
                if (account != null)
                {
                    //var item = cl["item1"].Equals("Auto");
                    string radioValue = cl["item1"];
                    if (radioValue == "auto")
                        account.auto = radioValue.Equals("auto");
                    if (radioValue == "remember")
                        account.remember = radioValue.Equals("remember");
                    myData.SubmitChanges();

                    try
                    {
                        if (radioValue.Equals("remember"))
                        {
                            userInfo["name"] = account.name;
                            userInfo.Expires = DateTime.Now.AddDays(365);
                            Response.Cookies.Add(userInfo);
                        }
                    }
                    catch (Exception)
                    {


                    }

                    try
                    {
                        if (radioValue.Equals("auto"))
                        {
                            userInfo["name"] = account.name;
                            userInfo["pass"] = account.pass;
                            userInfo.Expires = DateTime.Now.AddDays(365);
                            Response.Cookies.Add(userInfo);
                        }
                    }
                    catch (Exception)
                    {


                    }




                    //myData.TAIKHOANs.(account);

                    ViewBag.thongbao = radioValue + " " + userInfo["name"];
                    //return Login();
                    kiemtralogin = true;
                    return View();

                }
                else
                {
                    ViewBag.thongbao = " Đăng nhập thất bại";
                    return Login();
                }
            }
            return View();
        }

        private string a = "";

        public ActionResult Register()
        {
            ViewBag.a = a;
            return View();
        }

        [HttpPost]
        public ActionResult Register(TAIKHOAN t, FormCollection collect)
        {
            if (ModelState.IsValid)
            {

                var account = myData.TAIKHOANs.FirstOrDefault(r => r.name.Equals(t.name) || r.email.Equals(t.email));
                if (account != null)
                {
                    ViewBag.a = "Tài Khoản Đã Có người sử dụng, xin tạo email khác";
                }
                else
                {
                    var matkhau = myClass.MahoaMd5(t.pass);
                    var ms = collect["maso"];
                    if (ms.Equals("!!@@34"))
                    {
                        t.email = t.email;
                        t.name = t.name;
                        t.pass = myClass.MahoaMd5(matkhau);
                        myData.TAIKHOANs.InsertOnSubmit(t);
                        myData.SubmitChanges();
                        ModelState.Clear();
                        ViewBag.a = "Đăng Ký Thành Công";
                    }
                    else
                    {
                        ViewBag.a = "không đúng mã số, bạn không có quyền tạo tài khoản";
                        return View();
                    }
                }


                //ViewBag.a = ms;
            }
            return View(t);
        }

        public ActionResult ListVisa1()
        {
            return View(myData.ViSaPart1s.ToList());
        }

        //private List<ViSaPart1> TaoDsStram()
        //{

        //}
        public ActionResult QuanLyBangGia()
        {
            ViewBag.tablePrice1 = myData.ViSaPart1s.ToList();
            ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
            ViewBag.Purpose = myData.TPurposeofvisias.ToList();
            ViewBag.typeOfVisa = myData.TTypeofvisas.ToList();
            ViewBag.Port = myData.TArrivalPorts.ToList();
            ViewBag.processtime = myData.Processingtimes.ToList();
            ViewBag.qt = myData.Nationalities.ToList();
            ViewBag.typeOfVisa1 = myClass.DsTypeOfvisa();
            ViewBag.Numberint = myClass.ListNumberInt(1, 100, 1);
            ViewBag.alltypeOfVisa = myData.ViSaPart1s.Where(j => j.proTime.Equals("NORMAL CASE")).ToList();
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            ViewBag.processtimeView =
                myData.Processingtimes.Where(k => k.name != "Normal ").Select(j => j.name).Distinct().ToList();

            return View();
        }

        public ActionResult AddProTime(string name, string visit, int value)
        {
            myData.Processingtimes.InsertOnSubmit(new Processingtime()
            {
                name = name,
                _Purposeofvisit = visit,
                value = value.ToString()
            });
            myData.SubmitChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteProTime(int id)
        {
            var kt = myData.Processingtimes.FirstOrDefault(j => j.Id.Equals(id));
            if (kt != null)
            {
                myData.Processingtimes.DeleteOnSubmit(kt);
                myData.SubmitChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateProTime(int id, string name, string visit, int value)
        {
            var kt = myData.Processingtimes.FirstOrDefault(j => j.Id.Equals(id));
            if (kt != null)
            {
                kt.name = name;
                kt.value = value.ToString();
                kt._Purposeofvisit = visit;
                myData.SubmitChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private string data1 = "";

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult QuanLyBangGia(string data)
        {
            ViewBag.tablePrice1 = myData.ViSaPart1s.ToList();
            ViewBag.processtimeView =
                myData.Processingtimes.Where(k => k.name != "Normal ").Select(j => j.name).Distinct().ToList();
            //ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST") ).ToList();
            //ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS") ).ToList();
            data1 = data;
            Session["visa11"] = data;
            ViewBag.ter = "toi la ai " + Session["visa11"];
            return View();
        }

        public ActionResult DropDownVisaTourOrBiss(string data)
        {
            var item = myData.ViSaPart1s.Where(j => j.visit.Equals(data)).ToList();
            return Json(item, data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BangGiaVisa()
        {
            ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
            ViewBag.Purpose = myData.TPurposeofvisias.ToList();
            ViewBag.typeOfVisa = myData.TTypeofvisas.ToList();
            ViewBag.Port = myData.TArrivalPorts.ToList();
            ViewBag.processtime = myData.Processingtimes.Where(j => j.name != "Normal ").ToList();
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult BangGiaVisa(BGVISA vs)
        {
            if (ModelState.IsValid)
            {
                myData.BGVISAs.InsertOnSubmit(vs);
                myData.SubmitChanges();
            }
            ViewBag.numberofapplicant = myData.TNumberofapplicants.ToList();
            ViewBag.Purpose = myData.TPurposeofvisias.ToList();
            ViewBag.typeOfVisa = myData.TTypeofvisas.ToList();
            ViewBag.Port = myData.TArrivalPorts.ToList();
            ViewBag.processtime = myData.Processingtimes.ToList();
            return View();
        }

        private string DatabaseSelect(List<THotel> ht, List<TGiaPhong> gp, List<TLocaltion> lc,
            List<TTypeofvisa> ltypeofvisas, List<TPurposeofvisia> ltPurposeofvisias,
            List<TNumberofapplicant> laNumberofapplicants, List<Gender> lgGenders)
        {

            return "Thành Công";
        }

        public string GiaPhong1(double from, double to, string dv)
        {
            //    var item =
            //        myData.TGiaPhongs.FirstOrDefault(f => f.tu.Equals(from) && f.den.Equals(to) && f.donvitien.Equals(dv));
            myData.TGiaPhongs.InsertOnSubmit(new TGiaPhong() {ten = "From", tu = from, den = to, donvitien = dv});
            myData.SubmitChanges();
            ViewBag.giaphong = myData.TGiaPhongs.ToList();
            return "Oh";
        }

        public string DeleteGiaPhong(double from, double to, string dv)
        {
            var item =
                myData.TGiaPhongs.FirstOrDefault(f => f.tu.Equals(from) && f.den.Equals(to) && f.donvitien.Equals(dv));
            if (item != null) myData.TGiaPhongs.DeleteOnSubmit(item);
            myData.SubmitChanges();
            return "ok";
        }

        public string AddDiadiem(string dv)
        {
            //    var item =
            //        myData.TGiaPhongs.FirstOrDefault(f => f.tu.Equals(from) && f.den.Equals(to) && f.donvitien.Equals(dv));
            myData.TLocaltions.InsertOnSubmit(new TLocaltion() {name = dv});
            myData.SubmitChanges();
            ViewBag.giaphong = myData.TGiaPhongs.ToList();
            return "Oh";
        }

        public string DeleteDiadiem(string dv)
        {
            var item =
                myData.TLocaltions.FirstOrDefault(f => f.name.Equals(dv));
            if (item != null) myData.TLocaltions.DeleteOnSubmit(item);
            myData.SubmitChanges();
            return "ok";
        }

        private ActionResult GiaPhong()
        {
            var l = myData.TGiaPhongs.ToList();
            return Json(l, JsonRequestBehavior.AllowGet);
        }

        public string AddRom(string rom)
        {
            //    var item =
            //        myData.TGiaPhongs.FirstOrDefault(f => f.tu.Equals(from) && f.den.Equals(to) && f.donvitien.Equals(dv));
            myData.TRomtypes.InsertOnSubmit(new TRomtype() {tenphong = rom});
            myData.SubmitChanges();
            ViewBag.rom = myData.TRomtypes.ToList();
            return "Oh";
        }

        public string DeleteRom(string rom)
        {
            var item =
                myData.TRomtypes.FirstOrDefault(f => f.tenphong.Equals(rom));
            if (item != null) myData.TRomtypes.DeleteOnSubmit(item);
            myData.SubmitChanges();
            return "ok";
        }

        public string AddnumberofVisa(string text, int so)
        {
            //var item =
            //    myData.TGiaPhongs.FirstOrDefault(f => f.tu.Equals(from) && f.den.Equals(to) && f.donvitien.Equals(dv));
            myData.TNumberofapplicants.InsertOnSubmit(new TNumberofapplicant() {valuenumber = so, valuetext = text});
            myData.SubmitChanges();
            ViewBag.numberofvisa = myData.TNumberofapplicants.ToList();
            return "Oh";
        }

        public string Deletenumberofvisa(string text, int so)
        {
            var item =
                myData.TNumberofapplicants.FirstOrDefault(
                    f => f.valuetext.Equals(text.Trim()) && f.valuenumber.Equals(so));
            if (item != null) myData.TNumberofapplicants.DeleteOnSubmit(item);
            myData.SubmitChanges();
            ViewBag.numberofvisa = myData.TNumberofapplicants.ToList();
            return "Oh";
        }

        public ActionResult Step1Sum(FormCollection fomr)
        {
            Session["tienTv"] = 0;
            var numberPax = Shared.ParseInt(fomr["numberofapplicant"].Split(' ')[0]); // tong so thanh vien
            Session["sothanhvien"] = numberPax;
            var purposeofvisit = fomr["purposeofvisit"]; // du lich hay kinh doanh
            var typeofVisa = fomr["typeofvisa"]; // type of visa; 1 thang hay 
            var processingtime = fomr["processingtime"]; // cac kieu Normal, ugent
            var kt = new FEEVISA();
            var tientv = 0;
            var plus = new Processingtime();
            var tong = 0;
            if (processingtime.Contains("Urgent") || processingtime.Contains("Super Urgent"))
            {
                plus =
                    myData.Processingtimes.FirstOrDefault(
                        j => j.name.Contains(processingtime) && j._Purposeofvisit.Equals(purposeofvisit));
                Session["tienPlus"] = plus.value;

                kt =
                    myData.FEEVISAs.FirstOrDefault(
                        j => j.minPax == numberPax || j.minPax <= numberPax && numberPax <= j.maxPax.Value
                             && j.vist.Contains(purposeofvisit)
                             && j.typeofVisa.Contains(typeofVisa)
                             && j.serviceFee.Contains("NORMAL CASE"));
                if (kt != null)
                {
                    Session["tienTv"] = Shared.ParseInt(kt.value.ToString());
                    tong = (Shared.ParseInt(plus.value) + Shared.ParseInt(kt.value.ToString()))*numberPax;
                }


            }
            if (processingtime.Contains("Normal ") || processingtime.Contains("Holiday"))
            {


                kt =
                    myData.FEEVISAs.FirstOrDefault(
                        j => j.minPax == numberPax || j.minPax <= numberPax && numberPax <= j.maxPax
                             && j.vist.Contains(purposeofvisit)
                             && j.typeofVisa.Contains(typeofVisa)
                             && j.serviceFee.Contains(processingtime));
                if (kt != null)
                {
                    Session["tienTv"] = Shared.ParseInt(kt.value.ToString());
                    tong = Shared.ParseInt(kt.value.ToString())*numberPax;
                }

                Session["tienPlus"] = "0";

            }

            Session["tiendv"] = tong;
            return Json(tong, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XulyStep1(FormCollection fomr)
        {
            try
            {
                var numberPax = fomr["numberofapplicant"].Split(' ')[0];
                var purposeofvisit = fomr["purposeofvisit"];
                var typeofVisa = fomr["typeofvisa"];
                var processingtime = fomr["processingtime"];
                var phidv = ClassVisa.MoneyVisa(purposeofvisit, typeofVisa, Shared.ParseInt(numberPax))*
                            Shared.ParseInt(numberPax);

                //var kt =
                //    myData.ViSaPart1s.FirstOrDefault(
                //        j => j.typeOfVisa.Contains(typeofVisa) && j.visit.Contains(purposeofvisit));
                var plus =
                    myData.Processingtimes.FirstOrDefault(
                        j => j.name.Contains(processingtime) && j._Purposeofvisit.Equals(purposeofvisit));
                Session["tientyofvisa"] = ClassVisa.MoneyVisa(purposeofvisit, typeofVisa, Shared.ParseInt(numberPax));
                Session["tienTv"] = (phidv + Shared.ParseInt(plus.value)*Shared.ParseInt(numberPax))/
                                    Shared.ParseInt(numberPax);
                Session["tienPlus"] = ClassVisa.MoneyVisa(purposeofvisit, typeofVisa, Shared.ParseInt(numberPax));
                return Json(phidv + Shared.ParseInt(plus.value)*Shared.ParseInt(numberPax), JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {



            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PriviteVisaStep1(int t)
        {
            var kt = myData.APPLYCODEs.FirstOrDefault(j => j.id.Equals(t));
            if (kt != null)
            {
                Session["private"] = kt.value;
                return Json(kt.value, JsonRequestBehavior.AllowGet);

            }
            Session["private"] = 0;
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        DropDown dropDown = new DropDown();

        public ActionResult OnChangepurpose1Visa(string text)
        {
            var item
                =
                myData.ViSaPart1s.ToList()
                    .FindAll(j => j.pax1 > 0 && j.visit.ToLower().Contains(text.ToLower()))
                    .ToList();
            ViewBag.typeOfVisa = item;
            //ViewBag.typeOfVisa =dropDown.GetDropDown(item);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPriceVisaNormal(string typeofVisa, string visit)
        {
            var item =
                myData.ViSaPart1s.FirstOrDefault(h => h.typeOfVisa.Equals(typeofVisa) && h.visit.Equals(visit));

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadArrival()
        {
            var item =
                myData.TArrivalPorts.ToList();

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadGiaPhongHotel()
        {
            try
            {
                var item =
                    myData.TGiaPhongs.ToList();

                //ViewBag.typeOfVisa = item;
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadKieuphongHotel()
        {
            var item =
                myData.TRomtypes.ToList();

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BophantuCodulieuType(string typeofVisa, string visit)
        {
            var item =
                myData.ViSaPart1s.FirstOrDefault(
                    h => h.typeOfVisa.Equals(typeofVisa) && h.visit.Equals(visit) && h.pax1 == null);

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePriceVisa1(int id)
        {
            var tb = "";
            var k = myData.ViSaPart1s.FirstOrDefault(
                j => j.id.Equals(id));
            if (k != null)
            {
                myData.ViSaPart1s.DeleteOnSubmit(k);
                myData.SubmitChanges();
                tb = "Da xoa ";
            }
            else
            {
                tb = "Loi khong xoa duoc";
            }
            ViewBag.dulich = myData.ViSaPart1s.Where(j => j.visit.Equals("TOURIST")).ToList();
            ViewBag.doanhnhan = myData.ViSaPart1s.Where(j => j.visit.Equals("BUSINESS")).ToList();
            return Json(tb, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveHeadingVisa11(string p1ax, string p2ax, string p3ax, string p4ax, string p, int id)
        {
            var tb = "ko";
            if (ModelState.IsValid)
            {
                var k = myData.HeadingPaxes.FirstOrDefault(j => j.id.Equals(id));
                if (k != null)
                {
                    k.pax1 = p1ax;
                    k.pax2 = p2ax;
                    k.pax3 = p3ax;
                    k.pax4 = p4ax;
                    k.name = p;
                    myData.SubmitChanges();
                    tb = "Cập Nhật Thành Công";
                }
                else
                {
                    var n = new HeadingPax() {name = p, pax1 = p1ax, pax2 = p2ax, pax3 = p3ax, pax4 = p4ax,};
                    myData.HeadingPaxes.InsertOnSubmit(n);
                    myData.SubmitChanges();
                    tb = "Thêm Thành Công";
                }
            }


            return Json(tb, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadHeading()
        {
            var item =
                myData.HeadingPaxes.FirstOrDefault(h => h.id.Equals(1));

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnChangeProcessingtime(string text)
        {
            var item =
                myData.Processingtimes.Where(h => h._Purposeofvisit.Equals(text)).ToList();

            //ViewBag.typeOfVisa = item;
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View_AIRPORT_FAST_TRACK()
        {
            try
            {
                ViewBag.view_airport = myData.AIRPORTFASTRACKs.ToList();
                ViewBag.view_airportHeading = myData.NhomAirportFasts.ToList();
            }
            catch (Exception)
            {


            }
            return View();
        }

        public ActionResult View_CarPicker()
        {
            try
            {
                ViewBag.view_car = myData.CARPICKUPs.ToList();

            }
            catch (Exception)
            {


            }
            return View();
        }

        public ActionResult PayPaypal()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult KtSaveMember(string qt)
        {
            if (qt.Equals("Close"))
            {
                Session["specalDb"] = 0;
            }
            return Json(""+ Session["specalDb"], JsonRequestBehavior.AllowGet);
        }
        public ActionResult SpecalQt(string qt)
        {
            Session["specalDb"] = null;
            try
            {
                var id = long.Parse(Session["idtruongdoan"].ToString());
                var trdoan = myData.TRUONDOANs.FirstOrDefault(j => j.id.Equals(id));
                if (trdoan != null)
                {
                    var kt =
                        myData.FEEVISAs.FirstOrDefault(
                            j => j.specal.Equals(qt) && j.typeofVisa.Equals(trdoan.typeofvisa) && j.minPax.Value.Equals(1));
                    var qg = myData.Nationalities.FirstOrDefault(j => j.Name.Equals(qt));
                    if (kt != null && qg!=null)
                    {
                        Session["specalDb"] = kt.value;
                        return Json(kt.value + "," + qg.notes, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Session["specalDb"] = -1;
                    }
                }
            }
            catch (Exception)
            {
                
                
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}