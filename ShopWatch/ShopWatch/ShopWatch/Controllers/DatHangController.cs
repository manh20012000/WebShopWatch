using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
using ShopWatch.Payment;
namespace ShopWatch.Controllers
{
    [Route("api")]
    public class DatHangController : AllController
    {
        DHEntities db = new DHEntities();
        public ActionResult DonHang()
        {
            var email = Session["EmailClient"] as string;
            try
            {
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                int? khachhang = GetMaKH();
                if (khachhang == null)
                {
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
                DateTime ngayHienTai = DateTime.Now;
                var donhang = db.DATHANGs
                      .Where(m => m.MAKHACHHANG == user.MAKHACHHANG &&
                       (m.TINHTRANGDH=="" &&
                       DbFunctions.AddDays(m.NGAYNHAN, 5) < DateTime.Now));
                foreach (var item in donhang)
                {
                    item.TUYCHON = true;
                    item.NGAYNHAN = ngayHienTai;
                    item.TINHTRANGDH = "đã nhận hàng";
                }
                db.SaveChanges();
                var danhsachdonhang = db.DATHANGs
              .Where(m => m.MAKHACHHANG == user.MAKHACHHANG &&   // Chỉ lấy các đơn hàng của người dùng hiện tại
              (m.NGAYHUY == null || m.NGAYHUY > ngayHienTai) && // Điều kiện 1: Đơn hàng chưa bị hủy hoặc ngày hủy sau ngày hiện tại (không tính giờ và phút)
               (m.NGAYNHAN == null || DbFunctions.AddDays(m.NGAYNHAN, 5) >= ngayHienTai)) // Điều kiện 2: NGAYNHAN + 5 ngày lớn h
                .ToList();
                return View(danhsachdonhang);
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return View();
        }
        public ActionResult GiaohangIndex()
        {
            var email = Session["EmailClient"] as string;
            try
            {
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                int? khachhang = GetMaKH();
                if (khachhang == null)
                {
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
               

                var danhsachdonhang = db.DATHANGs
                           .Where(m => m.MAKHACHHANG == user.MAKHACHHANG &&
                            m.TUYCHON != false)
                          .ToList();
                return View(danhsachdonhang);

            }
            catch (Exception ex)
            {
                
            }
            return View();
        }
    

static string GenerateRandomString(int length, string characters)
    {
        Random random = new Random();
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            // Chọn một ký tự ngẫu nhiên từ chuỗi characters
            int index = random.Next(characters.Length);
            char randomChar = characters[index];

            // Thêm ký tự ngẫu nhiên vào chuỗi kết quả
            result.Append(randomChar);
        }

        return result.ToString();
    }

     [HttpPost]
        public ActionResult AccepDonhang(DATHANG dATHANG)
        {
            if (Session["EmailClient"] != null)
            {
                int length = 6;
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string randomString = GenerateRandomString(length, characters);
                var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                 if (dATHANG.HINHTHUCTHANHTOAN == false) // Kiểm tra hình thức thanh toán
                {
                    try { 
                    var diadiem = db.DIADIEMs.Find(dATHANG.MADIADIEM);
                     TRANGTHAIGIAOHANG giaohang = new TRANGTHAIGIAOHANG
                     {
                         MAVANDON = (int)dATHANG.MAVANDON,
                         VITRI = diadiem.TENDIACHI,
                         THOIGIANGIAOHANG = DateTime.Now,
                         THOIGIANNHANHANG=null,
                     };
                     db.TRANGTHAIGIAOHANGs.Add(giaohang);
                     DATHANG dathang = new DATHANG
                     {
                         HINHTHUCTHANHTOAN = false,
                         TONGTIEN = dATHANG.TONGTIEN,
                         MAKHACHHANG = dATHANG.MAKHACHHANG,
                         MADIADIEM = dATHANG.MADIADIEM,
                         MAQUANLYVOUCHER = dATHANG.MAQUANLYVOUCHER,
                         NGAYMUA = dATHANG.NGAYMUA,
                         MAVANDON = dATHANG.MAVANDON,
                         TRANGTHAI = false,
                         MADH = randomString,
                         TINHTRANGDH = "chờ xác nhận",
                     };
                     db.DATHANGs.Add(dathang);
                    DateTime StartDate = DateTime.Today;
                        DateTime EndDate = StartDate.AddYears(1);

                    foreach (var item in ItemsData)
                     {
                         CHITIETDATHANG ctdh = new CHITIETDATHANG
                         {
                             MAMATHANG = item.MAMATHANG,
                             SOLUONG = item.SOLUONG,
                             GIABAN = item.GIABAN,
                             MADH = randomString,
                         };
                         db.CHITIETDATHANGs.Add(ctdh);
                        var mathang = db.CHITIETGIOHANGs.FirstOrDefault(gh => gh.MAMATHANG == item.MAMATHANG);
                             db.CHITIETGIOHANGs.Remove(mathang);
                     }
                          if(dATHANG.MAQUANLYVOUCHER!=null){

                             var voucher = db.QUANLYVOUCHERs.Find(dATHANG.MAQUANLYVOUCHER);
                             voucher.TRANGTHAI = false;
                        }
                      
                        ViewBag.Voucherss = null;
                        var email = Session["EmailClient"] as string;
                        NOTIFICATION nOTIFICATION = new NOTIFICATION
                        {
                            MADH = dathang.MADH,
                            SENDID = dATHANG.MAKHACHHANG,
                            NOIDUNG = "Đã đặt hàng với ",
                            THOIGIAN = DateTime.Now,
                            EMAIL = email
                        };
                        db.NOTIFICATIONs.Add(nOTIFICATION);
                      
                      
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                       
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
                        hubContext.Clients.All.displayNotification("Thông báo từ Đơn hàng mới cần xác nhận  với mã ");
                        hubContext.Clients.All.SendNotification(nOTIFICATION); 
                      
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }
                     
                }
                return RedirectToAction("DonHang");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        [HttpPost]
        public ActionResult PayOnline(DATHANG Dathang, int TypePaymentVN)
        {
            var code = new { Success = false, Code = Dathang.HINHTHUCTHANHTOAN, Url = "" };
            if (ModelState.IsValid)
            {
                try
                {
                  if (Session["EmailClient"] != null)
                    {
                        int length = 6;
                        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        string randomString = GenerateRandomString(length, characters);
                        var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                        if (Dathang.HINHTHUCTHANHTOAN == true) // Kiểm tra hình thức thanh toán
                        {
                            var diadiems = db.DIADIEMs.Find(Dathang.MADIADIEM);
                            TRANGTHAIGIAOHANG giaohang = new TRANGTHAIGIAOHANG
                            {
                                MAVANDON = (int)Dathang.MAVANDON,
                                VITRI = diadiems.TENDIACHI,
                                THOIGIANGIAOHANG = DateTime.Now,
                                THOIGIANNHANHANG = null,
                            };
                            db.TRANGTHAIGIAOHANGs.Add(giaohang);
                            DATHANG dathang = new DATHANG
                            {
                                HINHTHUCTHANHTOAN = true,
                                TONGTIEN = Dathang.TONGTIEN,
                                MAKHACHHANG = Dathang.MAKHACHHANG,
                                MADIADIEM = Dathang.MADIADIEM,
                                MAQUANLYVOUCHER = Dathang.MAQUANLYVOUCHER,
                                NGAYMUA = Dathang.NGAYMUA,
                                MAVANDON = Dathang.MAVANDON,
                                TRANGTHAI = false,
                                MADH = randomString,
                                TINHTRANGDH = "chờ xác nhận",
                            };
                            db.DATHANGs.Add(dathang);
                            DateTime StartDate = DateTime.Today;
                            DateTime EndDate = StartDate.AddYears(1);
                            if (Dathang.TONGTIEN >= 1000000&&Dathang.TONGTIEN <= 2000000)
                            {
                                var vc = db.VOUCHERs.FirstOrDefault(vch => vch.PHANTRAMGIAMGIA == 4);
                                QUANLYVOUCHER newQuanlyVoucher = new QUANLYVOUCHER
                                {
                                    MAKHACHHANG = Dathang.MAKHACHHANG,
                                    MAVOUCHER = vc.MAVOUCHER,
                                    NGAYBATDAU = StartDate,
                                    NGAYKETTHUC = EndDate,
                                    TRANGTHAI = true,
                                }; db.QUANLYVOUCHERs.Add(newQuanlyVoucher);

                            }
                            else if (Dathang.TONGTIEN >= 20000000 && Dathang.TONGTIEN <= 5000000)
                            {
                                var vc = db.VOUCHERs.FirstOrDefault(vch => vch.PHANTRAMGIAMGIA == 5);
                                QUANLYVOUCHER newQuanlyVoucher = new QUANLYVOUCHER
                                {
                                    MAKHACHHANG = Dathang.MAKHACHHANG,
                                    MAVOUCHER = vc.MAVOUCHER,
                                    NGAYBATDAU = StartDate,
                                    NGAYKETTHUC = EndDate,
                                    TRANGTHAI = true,
                                }; db.QUANLYVOUCHERs.Add(newQuanlyVoucher);
                            }
                            else if (Dathang.TONGTIEN >= 5000000 && Dathang.TONGTIEN <= 9000000)
                            {
                                var vc = db.VOUCHERs.FirstOrDefault(vch => vch.PHANTRAMGIAMGIA == 3);
                                QUANLYVOUCHER newQuanlyVoucher = new QUANLYVOUCHER
                                {
                                    MAKHACHHANG = Dathang.MAKHACHHANG,
                                    MAVOUCHER = vc.MAVOUCHER,
                                    NGAYBATDAU = StartDate,
                                    NGAYKETTHUC = EndDate,
                                    TRANGTHAI = true,
                                }; db.QUANLYVOUCHERs.Add(newQuanlyVoucher);

                            }
                            else if (Dathang.TONGTIEN >= 100000000)
                            {
                                var vc = db.VOUCHERs.FirstOrDefault(vch => vch.PHANTRAMGIAMGIA == 12);
                                QUANLYVOUCHER newQuanlyVoucher = new QUANLYVOUCHER
                                {
                                    MAKHACHHANG = Dathang.MAKHACHHANG,
                                    MAVOUCHER = vc.MAVOUCHER,
                                    NGAYBATDAU = StartDate,
                                    NGAYKETTHUC = EndDate,
                                    TRANGTHAI = true,
                                };
                                db.QUANLYVOUCHERs.Add(newQuanlyVoucher);
                            }
                            foreach (var item in ItemsData)
                            {
                                CHITIETDATHANG ctdh = new CHITIETDATHANG
                                {
                                    MAMATHANG = item.MAMATHANG,
                                    SOLUONG = item.SOLUONG,
                                    GIABAN = item.GIABAN,
                                    MADH = randomString,
                                };
                                db.CHITIETDATHANGs.Add(ctdh);
                                var mathang = db.CHITIETGIOHANGs.FirstOrDefault(gh => gh.MAMATHANG == item.MAMATHANG);
                                db.CHITIETGIOHANGs.Remove(mathang);

                            }
                            if (Dathang.MAQUANLYVOUCHER != null)
                            {
                                var voucher = db.QUANLYVOUCHERs.Find(Dathang.MAQUANLYVOUCHER);
                                voucher.TRANGTHAI = false;
                            }
                            db.SaveChanges();
                            ViewBag.Voucherss = null;
                        }
                        code = new { Success = true, Code = Dathang.HINHTHUCTHANHTOAN, Url = "" };
                        //var url = "";

                        if (Dathang.HINHTHUCTHANHTOAN == true)
                        {
                            var url = UrlPayment(Dathang, TypePaymentVN, randomString);
                            code = new { Success = true, Code = Dathang.HINHTHUCTHANHTOAN, Url = url };
                        }

                    }
                    return Redirect(code.Url);
                }
                catch (Exception)
                {
                    
                }
            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        // thanh toán với payment vnpay
        public string  UrlPayment(DATHANG dathang, int typementVn,string MADH )
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input

          

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (dathang.TONGTIEN*100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (typementVn == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (typementVn == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (typementVn == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + MADH);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", MADH);
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount"))/100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.DATHANGs.FirstOrDefault(x => x.MADH == orderCode);
                        if (itemOrder != null)
                        {
                           
                            itemOrder.TRANGTHAI = true;//đã thanh toán
                            db.DATHANGs.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong

                        var email = Session["EmailClient"] as string;
                        // Gọi phương thức trong Hub và truyền thông báo
                        NOTIFICATION nOTIFICATION = new NOTIFICATION
                        {
                            MADH = orderCode,
                            SENDID = itemOrder.MAKHACHHANG,
                            NOIDUNG = "Đã đặt hàng với ",
                            THOIGIAN = DateTime.Now,
                            EMAIL= email
                        };

                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
                        hubContext.Clients.All.displayNotification("Thông báo từ Đơn hàng mới cần xác nhận  với mã " + orderCode);
                        hubContext.Clients.All.SendNotification(nOTIFICATION);
                        ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + @String.Format("{0:N0}", vnp_Amount.ToString());
                        db.NOTIFICATIONs.Add(nOTIFICATION);
                        db.SaveChanges();

                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        ViewBag.ThanhToanThanhCong = "bạn đã thanh toán lỗi với :" + vnp_Amount.ToString();
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            return View();
        }

        public ActionResult Details(string id)
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
            ViewBag.DataChitietdathang = db.CHITIETDATHANGs.Where(ctdh => ctdh.MADH == id).ToList();
            return View(dATHANG);
            }catch(Exception ex)
            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult Edit(string id)
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
            dATHANG.NGAYNHAN = DateTime.Now;
            dATHANG.TUYCHON = true;
            dATHANG.NGAYHUY=null;
            dATHANG.TINHTRANGDH = "đã nhận hàng";
            db.SaveChanges();
            return RedirectToAction("DonHang");
        }

        [HttpPost]
        public ActionResult CancelOrder(string id)
        {
            DATHANG dATHANG = db.DATHANGs.Find(id);
            if (dATHANG == null)
            {
                return HttpNotFound();
            }
            dATHANG.NGAYHUY = DateTime.Now;
            dATHANG.TINHTRANGDH = "đã hủy";
            dATHANG.TUYCHON = false;
            dATHANG.NGAYNHAN = null;
            
            db.SaveChanges();

            // Chuyển hướng về view DonHang
            return RedirectToAction("DonHang");
        }
      public ActionResult ListCanceOder()
        {
            var email = Session["EmailClient"] as string;
            try
            {
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                int? khachhang = GetMaKH();
                if (khachhang == null)
                {
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
                else
                {
                    var danhsachdonhang = db.DATHANGs.Where(m => m.NGAYHUY != null && m.MAKHACHHANG == user.MAKHACHHANG).ToList();
                    return View(danhsachdonhang);
                }
            }
            catch (Exception ex)
            {
           
            }

            return View();
        }
        public ActionResult ListOder()
        {
            var email = Session["EmailClient"] as string;
            try
            {
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                int? khachhang = GetMaKH();
                if (khachhang == null)
                {
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
                else
                {
                    var danhsachdonhang = db.DATHANGs.Where(m => m.NGAYNHAN != null && m.MAKHACHHANG == user.MAKHACHHANG).ToList();
                    return View(danhsachdonhang);
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        
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


/* public ActionResult DonHang(List<CHITIETDATHANG> selectedItemsData, double giatien)
        {
            if (Session["UserEmail"] != null)
            {
                Session["SelectedItemsData"] = selectedItemsData;
                var email = Session["UserEmail"] as string;
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var NGAYMUA = DateTime.Now;
                var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == user.MAKHACHHANG).ToList();
                Random random = new Random();
                int Numrd = random.Next(1000000, 9000000);
                ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG), "MAMATHANG", "TENHANG");
                DatHangMetaData dathang = new DatHangMetaData
                {
                    NGAYMUA = NGAYMUA,
                    KHACHHANG = user,
                    TONGTIEN = giatien,
                    MAVANDON = Numrd,
                };
                var datHangList = new List<DatHangMetaData> { dathang };
                return View("DonHang", datHangList);


            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");


        }*/
/*
public ActionResult AccepDonhang(double giatien)
{
    if (Session["EmailClient"] != null)
    {
        int length = 6;
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string randomString = GenerateRandomString(length, characters);
        var email = Session["EmailClient"] as string;
        var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
        var user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
        var quanLyVoucherList = db.QUANLYVOUCHERs
             .Where(quanLyVoucher => quanLyVoucher.MAKHACHHANG == user.MAKHACHHANG)
             .Include(quanLyVoucher => quanLyVoucher.KHACHHANG)
             .Include(quanLyVoucher => quanLyVoucher.VOUCHER);

        ViewBag.VOUCHER = new SelectList(quanLyVoucherList);
        ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG));
        if (ItemsData != null && ItemsData.Any())
        {

            DATHANG newHoaDon = new DATHANG
            {
                MADH = randomString,
                NGAYMUA = DateTime.Today,
                MAKHACHHANG = user.MAKHACHHANG,
                TONGTIEN = giatien,
                KHACHHANG = user,
            };

            foreach (var chitietdathang in ItemsData)
            {
                CHITIETDATHANG ctdhCreate = new CHITIETDATHANG
                {
                    MADH = randomString,
                    MAMATHANG = chitietdathang.MAMATHANG,
                    SOLUONG = chitietdathang.SOLUONG,
                    GIABAN = chitietdathang.GIABAN,

                };
                db.CHITIETDATHANGs.Add(ctdhCreate);
            }
            db.DATHANGs.Add(newHoaDon);

            db.SaveChanges();
            return RedirectToAction("DonHang", newHoaDon);
        }
    }
    return RedirectToAction("LoginUser", "TAIKHOANs");
}*/  /* public ActionResult DatHang(DATHANG dathang)
         {
             try
             {
                 int? id_khachhang = GetMaKH();
                 var khachhang = db.KHACHHANGs.Find(id_khachhang);
                 DATHANG newHoaDon = new DATHANG
                 {
                     NGAYMUA = DateTime.Today,
                     MAKHACHHANG = id_khachhang,
                     TRANGTHAI=dathang.TRANGTHAI,
                     TONGTIEN = dathang.TONGTIEN,

                 };
                 db.DATHANGs.Add(newHoaDon);
                 db.SaveChanges();

                string id_hoadon =" ";// lấy id hóa đơn để gán vào cthd 

                 var id_giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == id_khachhang);// lấy id giỏ hàng qua id khách
                 var danhsachgiohang = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == id_giohang.MAGIOHANG).ToList();// lấy ds sp qua id giohang
                 foreach (var item in danhsachgiohang)
                 {
                     *//*CHITIETHOADON ctdhCreate = new CHITIETHOADON
                     {
                         MAHD = id_hoadon,
                         MAMATHANG = item.MAMATHANG,
                         SOLUONG = item.SOLUONGMUA,
                         GIABAN = item.DONGIA,

                     };
                     db.CHITIETHOADONs.Add(ctdhCreate);
 *//*
                 }

                 db.SaveChanges();
                 db.CHITIETGIOHANGs.RemoveRange(danhsachgiohang);// xóa toàn bộ sp giỏ
                 var tongtien = db.CHITIETDATHANGs.Where(m => m.MADH == id_hoadon)// cập  nhật tiền
                                                 .Sum(m => m.GIABAN * m.SOLUONG);
                 var donhang = db.DATHANGs.Find(id_hoadon);
                 donhang.TONGTIEN = tongtien;
                 db.SaveChanges();
                 return RedirectToAction("DonHang");


             }
             catch (DbUpdateException ex)
             {
                 // In ra thông tin chi tiết về lỗi
                 Console.WriteLine(ex.Message);

                 // Nếu có inner exception, in ra thông tin chi tiết của nó
                 if (ex.InnerException != null)
                 {
                     Console.WriteLine("Inner Exception:");
                     Console.WriteLine(ex.InnerException.Message);
                 }
             }
             return RedirectToAction("form_DatHang", "GioHang");
         }*/ /*  var strSanPham = "";
                double thanhtien =0;
                double TongTien = 0;
                foreach (var sp in ItemsData)
                {
                    strSanPham += "<tr>";
                    strSanPham += "<td>" + sp.MAMATHANG + "</td>";
                    strSanPham += "<td>" + sp.SOLUONG + "</td>";
                    strSanPham += "<td>" + ShopWatch.Common.Common.FormatNumber(sp.GIABAN, 0) + "</td>";
                    strSanPham += "</tr>";
                    thanhtien = (double)Dathang.TONGTIEN;
                }
                var diadiem = db.DIADIEMs.Find(Dathang.MADIADIEM);
                TongTien = thanhtien;
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}", Dathang.MADH);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", Dathang.KHACHHANG.TENKHACHHANG);
                contentCustomer = contentCustomer.Replace("{{Phone}}", diadiem.SDT);
                contentCustomer = contentCustomer.Replace("{{Email}}", Dathang.KHACHHANG.EMAIL);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", diadiem.TENDIACHI);
                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Dathang.TONGTIEN.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", Dathang.TONGTIEN.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopWatch.Common.Common.FormatNumber(TongTien*100, 0));
                ShopWatch.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + Dathang.MADH, contentCustomer.ToString(), Dathang.KHACHHANG.EMAIL);
                string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}", Dathang.MADH);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", Dathang.KHACHHANG.TENKHACHHANG);
                contentCustomer = contentCustomer.Replace("{{Phone}}", diadiem.SDT);
                contentCustomer = contentCustomer.Replace("{{Email}}", Dathang.KHACHHANG.EMAIL);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}",diadiem.TENDIACHI);
                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", (Dathang.TONGTIEN * 100).ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", (Dathang.TONGTIEN * 100).ToString());
                ShopWatch.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + Dathang.MADH, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);*/