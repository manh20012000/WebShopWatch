using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
using ShopWatch.Payment;
namespace ShopWatch.Controllers
{
    public class ChatClientController : Controller
    {
        DHEntities db = new DHEntities();
        // GET: ChatClient
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Notification()
        {
            return View();
        }
        public ActionResult ChatClients()
        {
            var email = Session["EmailClient"] as string;
            KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);

            if (user != null)
            {
                ViewBag.UserId = user.MAKHACHHANG; // Truyền Id của người dùng vào ViewBag
                ViewBag.UserName = user.TENKHACHHANG; // Truyền tên của người dùng vào ViewBag
                ViewBag.avatar = user.AVATAR;                            // Các thông tin khác của người dùng có thể truyền vào ViewBag tại đây
            }

            return View();
        }
        // GET: ChatClient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChatClient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatClient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatClient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChatClient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatClient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChatClient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
