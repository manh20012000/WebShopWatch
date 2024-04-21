using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using PagedList;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using ShopWatch.Models.MetaDATA;
namespace ShopWatch.Areas.NhanVien.Controllers
{
      [Route("api")]
    public class MATHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/MATHANGs
        public ActionResult Product(string searchValue, string selectedType, string selectedBrand, string selectedPrice, string selectedSize, int page = 1)
        {
            int pageSize = 6;

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
                if (phanquyen == "NV QUANLYSP")
                {
                    var items = db.MATHANGs.Where(x => x.TRANGTHAI == false);
                    if (!string.IsNullOrEmpty(searchValue) || !string.IsNullOrEmpty(selectedBrand) || !string.IsNullOrEmpty(selectedSize) || !string.IsNullOrEmpty(selectedType) || !string.IsNullOrEmpty(selectedPrice))
                    {

                        if (!string.IsNullOrEmpty(selectedPrice))
                        {
                            double priceValue = double.Parse(selectedPrice);
                            items = items.Where(x =>
                                (selectedPrice == null || x.GIAHANG <= priceValue) && // So sánh giá tiền với giá trị đã chuyển đổi sang số
                                (searchValue == null || x.TENHANG.Contains(searchValue)) &&
                                (selectedSize == null || SqlFunctions.PatIndex("%" + selectedSize + "%", x.KICHTHUOC.ToString()) > 0) &&
                                (selectedBrand == null || x.TENHANGSANXUAT.Contains(selectedBrand)) &&
                                (selectedType == null || x.LOAI.Contains(selectedType))
                            );
                        }
                        else
                        {
                            items = items.Where(x =>
                                                      (searchValue == null || x.TENHANG.Contains(searchValue)) &&
                                                      (selectedSize == null || SqlFunctions.PatIndex("%" + selectedSize + "%", x.KICHTHUOC.ToString()) > 0) &&
                                                      (selectedBrand == null || x.TENHANGSANXUAT.Contains(selectedBrand)) &&
                                                      (selectedType == null || x.LOAI.Contains(selectedType))
                                                  );
                        }
                    }

                    return View(items.ToList().ToPagedList(page, pageSize));
                }

                return RedirectToAction("Index", "BackToPemission");
            }

            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult CreateMatHang()
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {

                    var Sales = db.SALEs
                    .Where(s => s.NGAYHETHAN >= DateTime.Now && s.TRANGTHAI != true).ToList();
                    ViewBag.MASALE = new SelectList(Sales, "MASALE", "MASALE");
                    return View();
            
            }
            return RedirectToAction("Index", "BackToPemission");
        }
            return RedirectToAction("LoginUser", "TAIKHOANs");
    }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateMatHang(MATHANG mathang)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var Sales = db.SALEs
                             .Where(s => s.NGAYHETHAN >= DateTime.Now && s.TRANGTHAI != true).ToList();
                             ViewBag.MASALE = new SelectList(Sales, "MASALE", "MASALE");
                            var reseult = db.MATHANGs.Find(mathang.MAMATHANG);
                            if (reseult == null) 
                            { var fImage = Request.Files["imageFile"];
                                if (fImage != null && fImage.ContentLength > 0)
                                {
                                    string fileName = fImage.FileName;
                                    string folderName = Server.MapPath("~/assets/Upload/" + mathang.MAMATHANG + fileName);
                                    fImage.SaveAs(folderName);
                                    mathang.TRANGTHAI = false;
                                    mathang.ANHSANPHAM = "/assets/Upload/" + mathang.MAMATHANG + fileName;
                                    db.MATHANGs.Add(mathang);
                                    var files = Request.Files.GetMultiple("ANHS");
                             
                                    foreach (var imagePath in files)
                                    {
                                        if (imagePath != null && imagePath.ContentLength > 0)
                                        {
                                            string folderName2 = Server.MapPath("~/assets/Upload/" + mathang.MAMATHANG + imagePath.FileName);
                                            imagePath.SaveAs(folderName2);
                                            var hinhAnh = new HINHANH
                                            {
                                                MAMATHANG = mathang.MAMATHANG,
                                                ANHS = "/assets/Upload/" + mathang.MAMATHANG + imagePath.FileName
                                            };
                                            db.HINHANHs.Add(hinhAnh);
                                        }
                                   
                                    } db.SaveChanges();
                                    /* return RedirectToAction("Product");*/
                                    TempData["ThongBao"] = "Sản phẩm đã được thêm thành công!";
                                    return RedirectToAction("CreateMatHang");
                                }
                               
                            } 
                            else
                            {
                                TempData["ThongBao"] = "Sản phẩm đã được tồn tại ";
                                /* kiểm tra để trả về thông báo mã hàng này đã tồn tại rồi */
                               
                            }
                           
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Console.WriteLine($"Property: {validationError.PropertyName} - Error: {validationError.ErrorMessage}");
                                }
                            }
                        }
                    }
                    return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        
        public ActionResult EditProduct(String id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                      MATHANG mathang = db.MATHANGs.Find(id);
                        var Sales = db.SALEs
                        .Where(s => s.NGAYHETHAN >= DateTime.Now && s.TRANGTHAI!=true).ToList();
                        ViewBag.MASALE = new SelectList(Sales, "MASALE", "MASALE");
                 
                    if (mathang == null)
                    {
                                return HttpNotFound();
                    }
                      return View(mathang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(MATHANG mathang)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    Console.WriteLine(mathang.MAMATHANG);
                var product = db.MATHANGs.Find(mathang.MAMATHANG);
                if (ModelState.IsValid)
                {
                    var fImage = Request.Files["imageFile"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = fImage.FileName;
                        string folderName = Server.MapPath("~/assets/Upload/" + fileName);
                        fImage.SaveAs(folderName);
                            product.ANHSANPHAM = "/assets/Upload/" + fileName;
                        }
                    product.KICHTHUOC = mathang.KICHTHUOC;
                    product.LOAI = mathang.LOAI;
                    product.NGAYSANXUAT = mathang.NGAYSANXUAT;
                    product.GIAHANG = mathang.GIAHANG;
                    product.TENHANG = mathang.TENHANG;
                    product.TENHANGSANXUAT = mathang.TENHANGSANXUAT;
                    product.BAOHANH = mathang.BAOHANH;
                        product.MASALE = mathang.MASALE;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
                    return RedirectToAction("Product", "MATHANGs");
                }
                return View(product);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        } 

        public ActionResult Delete(String id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MATHANG mATHANG = db.MATHANGs.Find(id);
                if (mATHANG == null)
                {
                    return HttpNotFound();
                }
                return View(mATHANG);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        // POST: NhanVien/MATHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public ActionResult DeleteConfirmed(String id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    MATHANG mathang = db.MATHANGs.Find(id);
            mathang.TRANGTHAI = true;
            db.SaveChanges();
            return RedirectToAction("Product");
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult Details(String id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    MATHANG mathang = db.MATHANGs.Find(id);
                    List<string> danhSachAnh = db.HINHANHs
                    .Where(h => h.MAMATHANG == id)
                    .Select(h => h.ANHS)
                    .ToList();
                    SALE sales = db.SALEs.Find(mathang.MASALE);
                    ViewBag.Sale = sales;
                    ViewBag.DanhSachAnh = danhSachAnh;
                    return View(mathang);
                }

                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        [HttpGet]
        public ActionResult GetSaleInfor(int saleId)
        {
            // Lấy thông tin SALE từ cơ sở dữ liệu hoặc từ nơi khác
            var sale = db.SALEs.Find(saleId);

            return Json(sale, JsonRequestBehavior.AllowGet);
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
/* public ActionResult CreateMatHang(MATHANG mathang)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            mathang.TRANGTHAI = false;
                            db.MATHANGs.Add(mathang);
                            var fImage = Request.Files["ANHS"];
                            foreach (var imagePath in mathang.ANHS)
                            {
                                string folderName = Server.MapPath("~/assets/Upload/" + imagePath);
                                var hinhAnh = new HINHANH
                                {
                                    MAMATHANG = mathang.MAMATHANG,
                                    ANHS = folderName
                                };
                                db.HINHANHs.Add(hinhAnh);
                            }
                            db.SaveChanges();
                            return RedirectToAction("Product");

                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Console.WriteLine($"Property: {validationError.PropertyName} - Error: {validationError.ErrorMessage}");
                                }
                            }
                        }
                    }
                    return View(mathang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        
        public ActionResult EditProduct(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MATHANG mathang = db.MATHANGs.Find(id);
                if (mathang == null)
                {
                    return HttpNotFound();
                }
                return View(mathang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
  if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            mathang.TRANGTHAI = false;
                            db.MATHANGs.Add(mathang);
                            var fImage = Request.Files["ANHS"];
                            foreach (var imagePath in mathang.ANHS)
                            {
                                string folderName = Server.MapPath("~/assets/Upload/" + imagePath);
                                var hinhAnh = new HINHANH
                                {
                                    MAMATHANG = mathang.MAMATHANG,
                                    ANHS = folderName
                                };
                                db.HINHANHs.Add(hinhAnh);
                            }
                            db.SaveChanges();
                            return RedirectToAction("Product");

                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    Console.WriteLine($"Property: {validationError.PropertyName} - Error: {validationError.ErrorMessage}");
                                }
                            }
                        }
                    }
                    return View(mathang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");*/