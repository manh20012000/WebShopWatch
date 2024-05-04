using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using ShopWatch.Models;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class NOTIFICATIONsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/NOTIFICATIONs public void Index()
        public ActionResult Index()
        {
            var nOTIFICATIONs = db.NOTIFICATIONs.Include(n => n.DATHANG).Include(n => n.KHACHHANG).ToList();
             return Json(nOTIFICATIONs, JsonRequestBehavior.AllowGet);/// Trả về dữ liệu dưới dạng JSON
            return View(nOTIFICATIONs);                           // Hoặc trả về PartialView: return PartialView("_NotificationList", nOTIFICATIONs);
        }

        public ActionResult GetData()
        {
            try
            {
                var notifications = db.NOTIFICATIONs
               .Include(n => n.DATHANG)
               .Include(n => n.KHACHHANG)
               .OrderByDescending(n => n.THOIGIAN)
               .ToList();

                List<object> notificationList = new List<object>();

                foreach (var notification in notifications)
                {      
                    var notificationObject = new
                    {
                        MANOTIFICATION = notification.MANOTIFICATION,
                        SENDID = notification.SENDID,
                        EMAIL = notification.EMAIL,
                        NOIDUNG = notification.NOIDUNG,
                        THOIGIAN = notification.THOIGIAN,
                        MADH= notification.MADH,

                    };

                    notificationList.Add(notificationObject);
                }

                return Json(notificationList, JsonRequestBehavior.AllowGet); // Trả về danh sách thông báo dưới dạng JSON
            
                /* return Json(notifications, JsonRequestBehavior.AllowGet);*/ // Trả về danh sách thông báo dưới dạng JSON
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                         // Hoặc trả về PartialView: return PartialView("_NotificationList", nOTIFICATIONs);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DATHANG dATHANG = db.DATHANGs.Find(id);
            if (dATHANG == null)
            {
                return HttpNotFound();
            }
            return View(dATHANG);
        }
        public ActionResult DetailDATHANG(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                DATHANG dATHANG = db.DATHANGs.Find(id);

                if (dATHANG == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DataChitietdathangAdmin = db.CHITIETDATHANGs.Where(ctdh => ctdh.MADH == id).ToList();
                return View(dATHANG);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


        }
        public ActionResult Xacnhan(string id, string emailUser)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                string userEmail = Session["UserEmail"] as string;
                var email = emailUser;
                if (!string.IsNullOrEmpty(email))
                {
                    // Gửi thông báo tới client dựa trên email
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
                    hubContext.Clients.User(email).displayNotification("Đơn hàng của bạn đã được xác nhận!");
                }
                NHANVIEN nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == userEmail);
                if (nhanvien != null)
                {
                    Session["MaNV"] = nhanvien.MANV;
                    Session["Avatar"] = nhanvien.AVATAR;
                }
                if (phanquyen == "NV HOTROKHACHHANG")
                {
                    var dATHANGs = db.DATHANGs.Find(id);
                    dATHANGs.TINHTRANGDH = "đang giao hàng";
                    /*var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
                    hubContext.Clients.User(email).SendNotificationToUser("Đơn hàng của bạn đã được xác nhận");*/
                    db.SaveChanges();
                    TempData["thongbaoxacnhan"] = id + " đơn hàng đã được xác nhận";

                    return RedirectToAction("Xacnhandonhang", "KHACHHANGs");
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");

        }
        // GET: NhanVien/NOTIFICATIONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(nOTIFICATION);
        }

        // GET: NhanVien/NOTIFICATIONs/Create
        public ActionResult Create()
        {
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "TINHTRANGDH");
            ViewBag.SENDID = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG");
            return View();
        }

        // POST: NhanVien/NOTIFICATIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANOTIFICATION,SENDID,EMAIL,NOIDUNG,THOIGIAN,MADH")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.NOTIFICATIONs.Add(nOTIFICATION);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "TINHTRANGDH", nOTIFICATION.MADH);
            ViewBag.SENDID = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", nOTIFICATION.SENDID);
            return View(nOTIFICATION);
        }

        // GET: NhanVien/NOTIFICATIONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            if (nOTIFICATION == null)
            {
                return HttpNotFound();
            }
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "TINHTRANGDH", nOTIFICATION.MADH);
            ViewBag.SENDID = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", nOTIFICATION.SENDID);
            return View(nOTIFICATION);
        }

        // POST: NhanVien/NOTIFICATIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANOTIFICATION,SENDID,EMAIL,NOIDUNG,THOIGIAN,MADH")] NOTIFICATION nOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOTIFICATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "TINHTRANGDH", nOTIFICATION.MADH);
            ViewBag.SENDID = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", nOTIFICATION.SENDID);
            return View(nOTIFICATION);
        }

        // GET: NhanVien/NOTIFICATIONs/Delete/5
        

        // POST: NhanVien/NOTIFICATIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NOTIFICATION nOTIFICATION = db.NOTIFICATIONs.Find(id);
            db.NOTIFICATIONs.Remove(nOTIFICATION);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
