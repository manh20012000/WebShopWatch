using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;

namespace ShopWatch.Controllers
{
    public class DIADIEMsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: DIADIEMs  var dIADIEMs = db.DIADIEMs.Include(d => d.KHACHHANG);
        public ActionResult Index()
        {
            if (Session["EmailClient"] != null)
            {
                string userEmail = Session["EmailClient"] as string;
                var khachhang = db.KHACHHANGs.FirstOrDefault(kh => kh.EMAIL == userEmail);
                var dIADIEMs = db.DIADIEMs
                 .Include(d => d.KHACHHANG)
                 .Where(d => d.MAKHACHHANG == khachhang.MAKHACHHANG)
                 .ToList();
                return View(dIADIEMs.ToList());
            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");
        }
        public ActionResult capnhatdiachi(int diadiemId)
        {
            // Tìm địa điểm có DIADIEM.MACDING = true
            var diadiemToUpdate = db.DIADIEMs.FirstOrDefault(d => d.MACDINH == true);
            if (diadiemToUpdate != null)
            {
                // Set DIADIEM.MACDING của địa điểm tìm được thành false
                diadiemToUpdate.MACDINH = false;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
             
            // Chuyển hướng đến action và controller mong muốn
            return RedirectToAction("Index", "DIADIEMs");
        }
        // GET: DIADIEMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIADIEM dIADIEM = db.DIADIEMs.Find(id);
            if (dIADIEM == null)
            {
                return HttpNotFound();
            }
            return View(dIADIEM);
        }

        // GET: DIADIEMs/Create
        public ActionResult Create()
        {
            if (Session["EmailClient"] != null)
            {
                string userEmail = Session["EmailClient"] as string;
                var khachhang = db.KHACHHANGs.FirstOrDefault(kh => kh.EMAIL == userEmail);

                ViewBag.KHANHHANG = khachhang;
                return View();
            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");
        }

        // POST: DIADIEMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADIADIEM,MAKHACHHANG,TENDIACHI,SDT")] DIADIEM dIADIEM)
        {
            if (ModelState.IsValid)
            {
                dIADIEM.MACDINH = false;
                db.DIADIEMs.Add(dIADIEM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dIADIEM.MAKHACHHANG);
            return View(dIADIEM);
        }

        // GET: DIADIEMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIADIEM dIADIEM = db.DIADIEMs.Find(id);
            if (dIADIEM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dIADIEM.MAKHACHHANG);
            return View(dIADIEM);
        }

        // POST: DIADIEMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADIADIEM,MAKHACHHANG,TENDIACHI,SDT")] DIADIEM dIADIEM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dIADIEM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dIADIEM.MAKHACHHANG);
            return View(dIADIEM);
        }

        // GET: DIADIEMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DIADIEM dIADIEM = db.DIADIEMs.Find(id);
            if (dIADIEM == null)
            {
                return HttpNotFound();
            }
            return View(dIADIEM);
        }

        // POST: DIADIEMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DIADIEM dIADIEM = db.DIADIEMs.Find(id);
            db.DIADIEMs.Remove(dIADIEM);
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
