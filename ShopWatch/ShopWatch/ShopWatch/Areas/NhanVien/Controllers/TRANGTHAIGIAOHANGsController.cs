using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopWatch.Models;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class TRANGTHAIGIAOHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/TRANGTHAIGIAOHANGs
        public ActionResult Index( int page=1)
        {
            int pageSize = 10;
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                string userEmail = Session["UserEmail"] as string;

                NHANVIEN nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == userEmail);
                if (nhanvien != null)
                {
                    Session["MaNV"] = nhanvien.MANV;
                    Session["Avatar"] = nhanvien.AVATAR;
                }
                if (phanquyen == "NV GIAOHANG")
                {
                    var items = db.TRANGTHAIGIAOHANGs.Where(x => x.TRANGTHAI == false);
                    return View(db.TRANGTHAIGIAOHANGs.ToList());
                    return View(items.ToList().ToPagedList(page, pageSize));
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");
           
        }

        // GET: NhanVien/TRANGTHAIGIAOHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG = db.TRANGTHAIGIAOHANGs.Find(id);
            if (tRANGTHAIGIAOHANG == null)
            {
                return HttpNotFound();
            }
            return View(tRANGTHAIGIAOHANG);
        }

        // GET: NhanVien/TRANGTHAIGIAOHANGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhanVien/TRANGTHAIGIAOHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAVANDON,VITRI,THOIGIANGIAOHANG,THOIGIANNHANHANG")] TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG)
        {
            if (ModelState.IsValid)
            {
                db.TRANGTHAIGIAOHANGs.Add(tRANGTHAIGIAOHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tRANGTHAIGIAOHANG);
        }

        // GET: NhanVien/TRANGTHAIGIAOHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG = db.TRANGTHAIGIAOHANGs.Find(id);
            if (tRANGTHAIGIAOHANG == null)
            {
                return HttpNotFound();
            }
            return View(tRANGTHAIGIAOHANG);
        }

        // POST: NhanVien/TRANGTHAIGIAOHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAVANDON,VITRI,THOIGIANGIAOHANG,THOIGIANNHANHANG")] TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tRANGTHAIGIAOHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tRANGTHAIGIAOHANG);
        }

        // GET: NhanVien/TRANGTHAIGIAOHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG = db.TRANGTHAIGIAOHANGs.Find(id);
            if (tRANGTHAIGIAOHANG == null)
            {
                return HttpNotFound();
            }
            return View(tRANGTHAIGIAOHANG);
        }

        // POST: NhanVien/TRANGTHAIGIAOHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TRANGTHAIGIAOHANG tRANGTHAIGIAOHANG = db.TRANGTHAIGIAOHANGs.Find(id);
            db.TRANGTHAIGIAOHANGs.Remove(tRANGTHAIGIAOHANG);
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
