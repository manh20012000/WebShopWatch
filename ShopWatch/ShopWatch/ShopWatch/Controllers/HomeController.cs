using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
namespace ShopWatch.Controllers
{
   
    public class HomeController : Controller
    {
        private DHEntities db = new DHEntities();
        public ActionResult Index()
        {
            var items = db.MATHANGs.Where(x => x.TRANGTHAI == false);
            /* select new MathangViewModel
             {
                 MAMATHANG = mathang.MAMATHANG,
                 TENHANG = mathang.TENHANG,
                 GIAHANG = mathang.GIAHANG,
                 NGAYSANXUAT = mathang.NGAYSANXUAT,
                 TENHANGSANXUAT = mathang.TENHANGSANXUAT,
                 BAOHANH = mathang.BAOHANH,
                 LOAI = mathang.LOAI,
                 KICHTHUOC = mathang.KICHTHUOC,
                 ANHSANPHAM = mathang.ANHSANPHAM,
                 SALE = mathang.SALE.TRANGTHAI == false ? mathang.SALE : null
             };*/


            return View(items.ToList());
        }
    
        public ActionResult homeIndex(string searchValue, string brand, string price, string size, string type, int page = 1)
        {
            if (Session["EmailClient"] != null)
            {
                int pageSize = 10;
                var items = db.MATHANGs.Where(x => x.TRANGTHAI == false);

                if (!string.IsNullOrEmpty(searchValue) || !string.IsNullOrEmpty(brand) || !string.IsNullOrEmpty(size) || !string.IsNullOrEmpty(type)|| !string.IsNullOrEmpty(price))
                {

                    if (!string.IsNullOrEmpty(price))
                    {
                        double priceValue = double.Parse(price);
                        items = items.Where(x =>
                            (price == null || x.GIAHANG <= priceValue) && // So sánh giá tiền với giá trị đã chuyển đổi sang số
                            (searchValue == null || x.TENHANG.Contains(searchValue)) &&
                            (size == null || SqlFunctions.PatIndex("%" + size + "%", x.KICHTHUOC.ToString()) > 0) &&
                            (brand == null || x.TENHANGSANXUAT.Contains(brand)) &&
                            (type == null || x.LOAI.Contains(type))
                        );
                    }
                    else
                    {
                        items = items.Where(x =>
                                                  (searchValue == null || x.TENHANG.Contains(searchValue)) &&
                                                  (size == null || SqlFunctions.PatIndex("%" + size + "%", x.KICHTHUOC.ToString()) > 0) &&
                                                  (brand == null || x.TENHANGSANXUAT.Contains(brand)) &&
                                                  (type == null || x.LOAI.Contains(type))
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
      
       
       public ActionResult danhsach()
        {
            return View();
        }
    }
}