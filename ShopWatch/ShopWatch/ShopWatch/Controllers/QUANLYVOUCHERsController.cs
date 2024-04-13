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
    public class QUANLYVOUCHERsController : Controller
    {
        private DHEntities db = new DHEntities();


        public ActionResult VoucherIndex()
        {
            if (Session["EmailClient"] != null)
            {
                var email = Session["EmailClient"] as string;
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var quanLyVoucherList = db.QUANLYVOUCHERs
                       .Where(quanLyVoucher => quanLyVoucher.MAKHACHHANG == user.MAKHACHHANG &&
                             quanLyVoucher.TRANGTHAI == true &&
                             quanLyVoucher.NGAYKETTHUC >= DateTime.Today)
                             .Include(quanLyVoucher => quanLyVoucher.KHACHHANG)
                             .Include(quanLyVoucher => quanLyVoucher.VOUCHER)
                             .ToList();

                return View(quanLyVoucherList);
            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");

        }
        public ActionResult Index()
        {
            var qUANLYVOUCHERs = db.QUANLYVOUCHERs.Include(q => q.KHACHHANG).Include(q => q.VOUCHER);
            return View(qUANLYVOUCHERs.ToList());
        }

        // GET: QUANLYVOUCHERs/Details/5
        public ActionResult MuaNgay(int? id)
        {
            if (Session["EmailClient"] != null)
            {
                 if (id == null)
                 {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                 }
             
                var voucher = db.QUANLYVOUCHERs.Find(id);
                Session["SessionQUANLYVOUCHER"] = voucher;
                return RedirectToAction("Index", "GioHang");
            }

            return RedirectToAction("Dangnhap", "TAIKHOANs");
         
        }

        // GET: QUANLYVOUCHERs/Create
        public ActionResult Create()
        {
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG");
            ViewBag.MAVOUCHER = new SelectList(db.VOUCHERs, "MAVOUCHER", "DIEUKIEN");
            return View();
        }

        // POST: QUANLYVOUCHERs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAQUANLYVOUCHER,MAKHACHHANG,MAVOUCHER,GHICHU,NOIDUNG,NGAYBATDAU,NGAYKETTHUC,TRANGTHAI")] QUANLYVOUCHER qUANLYVOUCHER)
        {
            if (ModelState.IsValid)
            {
                db.QUANLYVOUCHERs.Add(qUANLYVOUCHER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", qUANLYVOUCHER.MAKHACHHANG);
            ViewBag.MAVOUCHER = new SelectList(db.VOUCHERs, "MAVOUCHER", "DIEUKIEN", qUANLYVOUCHER.MAVOUCHER);
            return View(qUANLYVOUCHER);
        }

        // GET: QUANLYVOUCHERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANLYVOUCHER qUANLYVOUCHER = db.QUANLYVOUCHERs.Find(id);
            if (qUANLYVOUCHER == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", qUANLYVOUCHER.MAKHACHHANG);
            ViewBag.MAVOUCHER = new SelectList(db.VOUCHERs, "MAVOUCHER", "DIEUKIEN", qUANLYVOUCHER.MAVOUCHER);
            return View(qUANLYVOUCHER);
        }

        // POST: QUANLYVOUCHERs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAQUANLYVOUCHER,MAKHACHHANG,MAVOUCHER,GHICHU,NOIDUNG,NGAYBATDAU,NGAYKETTHUC,TRANGTHAI")] QUANLYVOUCHER qUANLYVOUCHER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qUANLYVOUCHER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", qUANLYVOUCHER.MAKHACHHANG);
            ViewBag.MAVOUCHER = new SelectList(db.VOUCHERs, "MAVOUCHER", "DIEUKIEN", qUANLYVOUCHER.MAVOUCHER);
            return View(qUANLYVOUCHER);
        }

        // GET: QUANLYVOUCHERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANLYVOUCHER qUANLYVOUCHER = db.QUANLYVOUCHERs.Find(id);
            if (qUANLYVOUCHER == null)
            {
                return HttpNotFound();
            }
            return View(qUANLYVOUCHER);
        }

        // POST: QUANLYVOUCHERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QUANLYVOUCHER qUANLYVOUCHER = db.QUANLYVOUCHERs.Find(id);
            db.QUANLYVOUCHERs.Remove(qUANLYVOUCHER);
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
