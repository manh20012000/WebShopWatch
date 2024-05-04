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
    public class DATHANGController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/DATHANG
         public ActionResult Index(int page = 1)
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
                    if (phanquyen == "NV KETOAN")
                    {
                        var dATHANGs = db.DATHANGs.Include(d => d.DIADIEM).Include(d => d.KHACHHANG).Include(d => d.QUANLYVOUCHER).Include(d => d.THANHTOAN);
                       
                        return View(dATHANGs.ToList().ToPagedList(page, pageSize));
                    }

                    return RedirectToAction("Index", "BackToPemission");
                }
                return RedirectToAction("LoginUser", "TAIKHOANs");        }
        public ActionResult listGiaohang(int page = 1)
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
                   
                    var dATHANGs = db.DATHANGs.Where(dh => dh.TINHTRANGDH == "đang giao hàng");

                    return View(dATHANGs.ToList().ToPagedList(page, pageSize));
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");


        }
   
        // GET: NhanVien/DATHANG/Details/5
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

        // GET: NhanVien/DATHANG/Create
        public ActionResult Create()
        {
            ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI");
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG");
            ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU");
            ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN");
            ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI");
            return View();
        }

        // POST: NhanVien/DATHANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADH,NGAYMUA,TONGTIEN,MAKHACHHANG,TRANGTHAI,MADIADIEM,MAQUANLYVOUCHER,MATHANHTOAN,MAVANDON,HINHTHUCTHANHTOAN")] DATHANG dATHANG)
        {
            if (ModelState.IsValid)
            {
                db.DATHANGs.Add(dATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI", dATHANG.MADIADIEM);
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dATHANG.MAKHACHHANG);
            ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU", dATHANG.MAQUANLYVOUCHER);
            ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN", dATHANG.MATHANHTOAN);
            ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI", dATHANG.MAVANDON);
            return View(dATHANG);
        }

        // GET: NhanVien/DATHANG/Edit/5
        public ActionResult Edit(string id)
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
                dATHANG.NGAYNHAN = DateTime.Now;
                dATHANG.TUYCHON = true;
                dATHANG.TINHTRANGDH = "giao thanh công";
                db.SaveChanges();
                return RedirectToAction("listGiaohang", "DATHANG");

            }
            catch (Exception)
            {

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        // POST: NhanVien/DATHANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADH,NGAYMUA,TONGTIEN,MAKHACHHANG,TRANGTHAI,MADIADIEM,MAQUANLYVOUCHER,MATHANHTOAN,MAVANDON,HINHTHUCTHANHTOAN")] DATHANG dATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI", dATHANG.MADIADIEM);
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dATHANG.MAKHACHHANG);
            ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU", dATHANG.MAQUANLYVOUCHER);
            ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN", dATHANG.MATHANHTOAN);
            ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI", dATHANG.MAVANDON);
            return View(dATHANG);
        }

        // GET: NhanVien/DATHANG/Delete/5
        public ActionResult Delete(string id)
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

        // POST: NhanVien/DATHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DATHANG dATHANG = db.DATHANGs.Find(id);
            db.DATHANGs.Remove(dATHANG);
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
