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
    public class KHACHHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/KHACHHANGs
        public ActionResult Index()
        {
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
                if (phanquyen == "NV HOTROKHACHHANG")
                {
                    var kHACHHANGs = db.KHACHHANGs.Include(k => k.TAIKHOAN);
                     return View(kHACHHANGs.ToList());
                }
                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");
        
    }
        // GET: NhanVien/KHACHHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // GET: NhanVien/KHACHHANGs/Create
        public ActionResult Create()
        {
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU");
            return View();
        }

        // POST: NhanVien/KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAKHACHHANG,TENKHACHHANG,SDT,EMAIL,AVATAR,THANHVIEN,XU")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.Add(kHACHHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", kHACHHANG.EMAIL);
            return View(kHACHHANG);
        }

        // GET: NhanVien/KHACHHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", kHACHHANG.EMAIL);
            return View(kHACHHANG);
        }
        public ActionResult Xacnhan(string id)
        {
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
                if (phanquyen == "NV HOTROKHACHHANG")
                {
                    var dATHANGs = db.DATHANGs.Find(id);
                    dATHANGs.TINHTRANGDH = "đang giao hàng";
                    db.SaveChanges();
                    return RedirectToAction("Xacnhandonhang", "KHACHHANGs");
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");

        }
        // POST: NhanVien/KHACHHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAKHACHHANG,TENKHACHHANG,SDT,EMAIL,AVATAR,THANHVIEN,XU")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACHHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", kHACHHANG.EMAIL);
            return View(kHACHHANG);
        }
        public ActionResult Xacnhandonhang(int page = 1)
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
                if (phanquyen == "NV HOTROKHACHHANG")
                {
                    var dATHANGs = db.DATHANGs.Where(dd => dd.TINHTRANGDH == "chờ xác nhận");

                    return View(dATHANGs.ToList().ToPagedList(page, pageSize));
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");


        }
        // GET: NhanVien/KHACHHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: NhanVien/KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            db.KHACHHANGs.Remove(kHACHHANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult INFORODER(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
                var danhsachdonhang = db.DATHANGs
                          .Where(m => m.MAKHACHHANG == kHACHHANG.MAKHACHHANG)
                         .ToList();
                return View(danhsachdonhang);
            }
            catch
            {

            }
              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult DetailDATHANG(string  id)
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
