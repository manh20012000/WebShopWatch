using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class SIGNARLsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/SIGNARLs
        public ActionResult Index()
        {
            var sIGNARLs = db.SIGNARLs.Include(s => s.KHACHHANG);
            return View(sIGNARLs.ToList());
        }

        // GET: NhanVien/SIGNARLs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGNARL sIGNARL = db.SIGNARLs.Find(id);
            if (sIGNARL == null)
            {
                return HttpNotFound();
            }
            return View(sIGNARL);
        }

        // GET: NhanVien/SIGNARLs/Create
        public ActionResult Create()
        {
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG");
            return View();
        }

        // POST: NhanVien/SIGNARLs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MAKHACHHANG,CONTEN,TITLE,TIME")] SIGNARL sIGNARL)
        {
            if (ModelState.IsValid)
            {
                db.SIGNARLs.Add(sIGNARL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", sIGNARL.MAKHACHHANG);
            return View(sIGNARL);
        }

        // GET: NhanVien/SIGNARLs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGNARL sIGNARL = db.SIGNARLs.Find(id);
            if (sIGNARL == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", sIGNARL.MAKHACHHANG);
            return View(sIGNARL);
        }

        // POST: NhanVien/SIGNARLs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MAKHACHHANG,CONTEN,TITLE,TIME")] SIGNARL sIGNARL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIGNARL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", sIGNARL.MAKHACHHANG);
            return View(sIGNARL);
        }

        // GET: NhanVien/SIGNARLs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIGNARL sIGNARL = db.SIGNARLs.Find(id);
            if (sIGNARL == null)
            {
                return HttpNotFound();
            }
            return View(sIGNARL);
        }

        // POST: NhanVien/SIGNARLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIGNARL sIGNARL = db.SIGNARLs.Find(id);
            db.SIGNARLs.Remove(sIGNARL);
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
