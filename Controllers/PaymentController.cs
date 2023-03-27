using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Uni_Shop.Service;
using Uni_Shop.Models;
using System.Collections.Specialized;
using Uni_Shop.ModelDBs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PayPal.Core;
using PayPal.v1.Payments;
using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;

namespace Uni_Shop.Controllers
{
    public class PaymentController : Controller
    {
        /* [Route("/payment/vnpay", Name = "/payment/vnpay")]*/
        TN230_V1Context db = new TN230_V1Context();

        private readonly ILogger<PaymentController> _logger;
        private readonly INotyfService _notyf;
        private readonly string _clientId;
        private readonly string _secretKey;
        public double TyGiaUSD = 23300;//store in Database
        public PaymentController(ILogger<PaymentController> logger, INotyfService notyf, IConfiguration config)
        {
            _logger = logger;
            _notyf = notyf;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }
        public ActionResult Payment(CheckOut checkOut)
        {
            //Khoa moi sua ngay 19h40 ngay 13/11
            var nd = HttpContext.Session.GetInt32("mand");
            var taikhoan = db.NguoiDungs.Where(s => s.MaNguoiDung == nd).FirstOrDefault();
            if(taikhoan.TenNguoiDung == null)
            {
                return Redirect("/profile");
            }
            var total = HttpContext.Session.GetInt32("thanhtien") * 100;
            string url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string returnUrl = "https://localhost:44369/Payment/PaymentConfirm";
            string message = "Đơn hàng đã được đặt!";
            string tmnCode = "Q4WFEGCI";
            string hashSecret = "AJSCZHOSJGLAWHQFOOXLELGGBVARHSKS";

            PaymentServices pay = new PaymentServices();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_Amount", total.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_IpAddr", PaymentServices.GetIpAddress(HttpContext)); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY




            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public async Task<IActionResult> PaymentConfirm()
        {
            DonDat donhang = new DonDat();
            List<GioHang> giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();

            if (Request.QueryString.Value.Count() != 0)
            {
                string hashSecret = "AJSCZHOSJGLAWHQFOOXLELGGBVARHSKS";//Chuỗi bí mật
                NameValueCollection vnpayData = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                PaymentServices pay = new PaymentServices();
                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                        ViewBag.vnpayTranId = vnpayTranId;
                       /* Thêm trường mã thanh toán trong csdl*/
                             donhang.MaGd = vnpayTranId.ToString();
                        donhang.MaHttt = 1;

                        donhang.MaNguoiDung = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s.MaNguoiDung).FirstOrDefault();
                        donhang.MaTrangThai = 1;
                        donhang.NgayDatHang = DateTime.Now.ToString();
                        db.Add(donhang);
                        await db.SaveChangesAsync();
                        _notyf.Success("Cập nhật đơn hàng thành công!");

                        List<NongSan> nongsans = db.NongSans.ToList();
                        List<AnhN> anhs = db.AnhNs.ToList();
                        var cartcontent = from g in giohangs
                                          join n in nongsans on g.MaNongSan equals n.MaNongSan into table1
                                          from n in table1.DefaultIfEmpty()
                                          join a in anhs on n.MaNongSan equals a.MaNongSan into table2
                                          from a in table2.DefaultIfEmpty()
                                          select new CartContent
                                          {
                                              nongsandetail = n,
                                              anhnongsandeltail = a,
                                              SL = (int)g.Sl

                                          };
                        NguoiDung nguoidungcontent = (from s in db.NguoiDungs where s.MaTaiKhoan == HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
                        var checkout = new CheckOut();
                        checkout.cartcontentdetail = cartcontent;
                        checkout.nguoidungdetail = nguoidungcontent;

                        var madondat = (from s in db.DonDats.OrderByDescending(x => x.MaDonDat) where s.MaNguoiDung == (int)HttpContext.Session.GetInt32("mand").Value select s.MaDonDat).FirstOrDefault();
                        giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
                        foreach (var element in giohangs)
                        {
                            ChiTietNsDd item = new ChiTietNsDd();
                            item.MaDonDat = madondat;
                            item.MaNongSan = element.MaNongSan;
                            item.SoLuongDat = element.Sl;
                            db.Add(item);
                            await db.SaveChangesAsync();
                            db.Remove(element);
                            await db.SaveChangesAsync();
                        }
                        int count = db.GioHangs.Where(s => s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK")).Count();
                        HttpContext.Session.SetInt32("countCart", count);
                        await db.SaveChangesAsync();

                        TaiKhoan taikhoan = (from s in db.TaiKhoans where s.MaTaiKhoan == HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
                        var result = new EmailController().SendMail("UniShop-Confirm Other", "order_information", new string[1] { taikhoan.Email }, null, checkout, "Ví  điện tử VNPAY");
                        
                        return View(checkout);
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode

                        _notyf.Error("Có lỗi xảy ra trong quá trình xử lý hóa đơn hàng! Thử lại sau...!");
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                        return Redirect("/cart");
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login");
            }

            return View(donhang);
        }

        public IActionResult CheckoutFail()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Chưa thanh toán"
            //Xóa session
            return View();
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            DonDat donhang = new DonDat();
            List<GioHang> giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();

            donhang.MaNguoiDung = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s.MaNguoiDung).FirstOrDefault();
            donhang.MaTrangThai = 1;
            donhang.NgayDatHang = DateTime.Now.ToString();
            donhang.MaGd = HttpContext.Session.GetString("paypalId").ToString();
            donhang.MaHttt = 2;
            db.Add(donhang);
            await db.SaveChangesAsync();
            _notyf.Success("Cập nhật đơn hàng thành công!");

            List<NongSan> nongsans = db.NongSans.ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            var cartcontent = from g in giohangs
                              join n in nongsans on g.MaNongSan equals n.MaNongSan into table1
                              from n in table1.DefaultIfEmpty()
                              join a in anhs on n.MaNongSan equals a.MaNongSan into table2
                              from a in table2.DefaultIfEmpty()
                              select new CartContent
                              {
                                  nongsandetail = n,
                                  anhnongsandeltail = a,
                                  SL = (int)g.Sl

                              };
            NguoiDung nguoidungcontent = (from s in db.NguoiDungs where s.MaTaiKhoan == HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
            var checkout = new CheckOut();
            checkout.cartcontentdetail = cartcontent;
            checkout.nguoidungdetail = nguoidungcontent;

            var madondat = (from s in db.DonDats.OrderByDescending(x => x.MaDonDat) where s.MaNguoiDung == (int)HttpContext.Session.GetInt32("mand").Value select s.MaDonDat).FirstOrDefault();
            giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
            foreach (var element in giohangs)
            {
                ChiTietNsDd item = new ChiTietNsDd();
                item.MaDonDat = madondat;
                item.MaNongSan = element.MaNongSan;
                item.SoLuongDat = element.Sl;
                db.Add(item);
                await db.SaveChangesAsync();
                db.Remove(element);
                await db.SaveChangesAsync();
            }
            ViewBag.PayPalId = HttpContext.Session.GetString("paypalId");
            int count = db.GioHangs.Where(s => s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK")).Count();
            HttpContext.Session.SetInt32("countCart", count);
            await db.SaveChangesAsync();
            TaiKhoan taikhoan = (from s in db.TaiKhoans where s.MaTaiKhoan == HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
            var result = new EmailController().SendMail("UniShop-Confirm Other", "order_information", new string[1] {taikhoan.Email }, null, checkout, "Ví  điện tử PAYPAL");


            return View(checkout);
        }

        public async Task<IActionResult> PaypalCheckout()

        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            List<GioHang> cartContents = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
            var total = Math.Round(HttpContext.Session.GetInt32("thanhtien").Value / TyGiaUSD, 1);
            foreach (var item in cartContents)
            {
                NongSan ns = (from s in db.NongSans where s.MaNongSan == item.MaNongSan select s).FirstOrDefault();
                itemList.Items.Add(new Item()
                {
                    Name = ns.TenNongSan.ToString(),
                    Currency = "USD",
                    Price = Math.Round(float.Parse(ns.Gia) / TyGiaUSD, 1).ToString(),
                    Quantity = item.Sl.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
                total += (double)(Math.Round(float.Parse(ns.Gia) / TyGiaUSD, 1) * item.Sl);
            }
            total -= Math.Round(HttpContext.Session.GetInt32("thanhtien").Value / TyGiaUSD, 1);
            #endregion

            var paypalOrderId = DateTime.Now.Ticks;

            HttpContext.Session.SetString("paypalId", paypalOrderId.ToString());
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Payment/CheckoutFail",
                    ReturnUrl = $"{hostname}/Payment/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Payment/CheckoutFail");
            }
        }
    }
}























    /* public ActionResult Payment()
     {
         //request params need to request to MoMo system
         string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
         string partnerCode = "MOMOOJOI20210710";
         string accessKey = "iPXneGmrJH0G8FOP";
         string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
         string orderInfo = "test";
         string returnUrl = "https://localhost:44369/Payment/ConfirmPaymentClient";
         string notifyurl = "https://localhost:44369/Payment/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

         string amount = "1000";
         string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
         string requestId = DateTime.Now.Ticks.ToString();
         string extraData = "";

         //Before sign HMAC SHA256 signature
         string rawHash = "partnerCode=" +
             partnerCode + "&accessKey=" +
             accessKey + "&requestId=" +
             requestId + "&amount=" +
             amount + "&orderId=" +
             orderid + "&orderInfo=" +
             orderInfo + "&returnUrl=" +
             returnUrl + "&notifyUrl=" +
             notifyurl + "&extraData=" +
             extraData;

         MoMoSecurity crypto = new MoMoSecurity();
         //sign signature SHA256
         string signature = crypto.signSHA256(rawHash, serectkey);

         //build body json request
         JObject message = new JObject
         {
             { "partnerCode", partnerCode },
             { "accessKey", accessKey },
             { "requestId", requestId },
             { "amount", amount },
             { "orderId", orderid },
             { "orderInfo", orderInfo },
             { "returnUrl", returnUrl },
             { "notifyUrl", notifyurl },
             { "extraData", extraData },
             { "requestType", "captureMoMoWallet" },
             { "signature", signature }

         };

         string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

         JObject jmessage = JObject.Parse(responseFromMomo);

         return Redirect(jmessage.GetValue("payUrl").ToString());
     }

     //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
     //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
     //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
     public ActionResult ConfirmPaymentClient(Momo result)
     {
         //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
         string rMessage = result.message;
         string rOrderId = result.orderId;
         string rErrorCode = result.errorCode; // = 0: thanh toán thành công
         return View();
     }

     [HttpPost]
     public void SavePayment()
     {
         //cập nhật dữ liệu vào db
         String a = "";
     }*/


