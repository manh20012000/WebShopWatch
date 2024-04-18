using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopWatch.Models;
namespace ShopWatch.Controllers
{
    public class MatHangController : Controller
    {
        DHEntities db = new DHEntities();
        // GET: MatHang
        public ActionResult Index()
        {
            var items = db.MATHANGs.Where(x => x.TRANGTHAI == false);
            return View(items.ToList());
        }
        [HttpGet]
        public ActionResult Chitietmathang(string id)
        {
            var sanpham = db.MATHANGs.Find(id);
            if (sanpham != null)
            {
                ViewBag.hinhanhs = db.HINHANHs.Where(m => m.MAMATHANG == id).Take(3).ToList();
                return View(sanpham);
            }

            return RedirectToAction("homeIndex", "Home");
        }
          public ActionResult mathang(string searchValue, string selectedType, string selectedBrand, string selectedPrice, string selectedSize, int page = 1)
        { 
            if (Session["EmailClient"] != null)
            {
                int pageSize = 10;
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
                else
                {
                    var pagedData = items.ToList().ToPagedList(page, pageSize);
                    return View(pagedData);
                }

                return View(items.ToList().ToPagedList(page, pageSize));

            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");

        }
        public ActionResult homeIndex( int page = 1)
        {
                int pageSize = 10;
                var items = db.MATHANGs.Where(x => x.TRANGTHAI == false);
                return View(items.ToList().ToPagedList(page, pageSize));

        }

        public ActionResult danhsach()
        {
            return View();
        }

    }

}