using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;

namespace Uni_Shop.Controllers
{
    public class HomeController : Controller
    {

        TN230_V1Context db = new TN230_V1Context();

        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        //alert("@ViewBag.Message");

        public HomeController(ILogger<HomeController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        [Route("/", Name = "")]
        public IActionResult Index()
        {
            int i = 1;
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = db.NongSans.ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            var nongsan = from n in nongsans
                          join a in anhs on n.MaNongSan equals a.MaNongSan
                          where n.TrangThai == 1
                          select new NongSanContent
                          {
                              nongsandetail = n,
                              anhnongsandeltail = a
                          };
            nongsan = nongsan.Take(16);
            
            return View(nongsan);
        }

        [Route("/filter", Name = "filter")]
        public IActionResult Filter(int pg = 1, string SearchText = "")
        {
            List<LoaiNongSan> loainongsan = db.LoaiNongSans.ToList();
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = db.NongSans.ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            IEnumerable<NongSanContent> nongsan;
            if (SearchText != "" && SearchText != null)
            {
                nongsan = from s in nongsans
                          join a in anhs on s.MaNongSan equals a.MaNongSan into table2
                          from a in table2.DefaultIfEmpty()
                          join g in gianhangs on s.MaGianHang equals g.MaGianHang into table0
                          from g in table0.DefaultIfEmpty()
                          join h in donViTinhs on s.MaDonViTinh equals h.MaDonViTinh into table1
                          from h in table1.DefaultIfEmpty()
                          join f in loainongsans on s.MaLoaiNongSan equals f.MaLoaiNongSan
                          where s.TenNongSan.Contains(SearchText)
                          select new NongSanContent
                          {
                              nongsandetail = s,
                              gianhangdetail = g,
                              loainongsandeltail = f,
                              donvitinhdetail = h,
                              anhnongsandeltail = a
                          };
            } else
            {
                    nongsan = from s in nongsans
                          join a in anhs on s.MaNongSan equals a.MaNongSan into table2
                          from a in table2.DefaultIfEmpty()
                          join g in gianhangs on s.MaGianHang equals g.MaGianHang into table0
                          from g in table0.DefaultIfEmpty()
                          join h in donViTinhs on s.MaDonViTinh equals h.MaDonViTinh into table1
                          from h in table1.DefaultIfEmpty()
                          join f in loainongsans on s.MaLoaiNongSan equals f.MaLoaiNongSan
                          select new NongSanContent
                          {
                              nongsandetail = s,
                              gianhangdetail = g,
                              loainongsandeltail = f,
                              donvitinhdetail = h,
                              anhnongsandeltail = a
                          };
            }
            
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = nongsan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nongsan.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.nongsan = data;
            return View(loainongsan);
        }

        [Route("/list", Name = "list")]
        public IActionResult List()
        {
            List<LoaiNongSan> loainongsan = db.LoaiNongSans.ToList();
            
            return View(loainongsan);
        }
        [Route("/list/{maloai}", Name = "ShopList")]
        public IActionResult ShopList(int maloai, int pg=1)
        {
            int i = 1;
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = db.NongSans.Where(x => x.MaLoaiNongSan == maloai).ToList();        
            List<AnhN> anhs = db.AnhNs.ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            var nongsan = from s in nongsans
                          join a in anhs on s.MaNongSan equals a.MaNongSan into table2
                          from a in table2.DefaultIfEmpty()
                          join g in gianhangs on s.MaGianHang equals g.MaGianHang into table0
                          from g in table0.DefaultIfEmpty()
                          join h in donViTinhs on s.MaDonViTinh equals h.MaDonViTinh into table1
                          from h in table1.DefaultIfEmpty()
                          join f in loainongsans on s.MaLoaiNongSan equals f.MaLoaiNongSan
                          where s.TrangThai == i
                          select new NongSanContent
                          {
                              nongsandetail = s,
                              gianhangdetail = g,
                              loainongsandeltail = f,
                              donvitinhdetail = h,
                              anhnongsandeltail = a
                          };
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = nongsan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nongsan.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.nongsan = data;
            return View(nongsan);
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("remember") != null)
            {
                return RedirectPermanentPreserveMethod("/");
            }
            LoginModel model = new LoginModel();
            return View(model);
        }
        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TenDangNhap == null)
                {
                    ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập! ");
                    return View();
                }
                if (model.MatKhau == null)
                {
                    ModelState.AddModelError("", "Vui lòng nhập mật khẩu! ");
                    return View();
                }
                TaiKhoan res = new TaiKhoan();
                var tk = db.TaiKhoans.Where(s => s.TenDangNhap == model.TenDangNhap.Trim()).SingleOrDefault();
                var mahoa = EncodeManager.VerifyHashedPassword(tk.MatKhau, model.MatKhau);
                if (mahoa == PasswordVerificationResult.Success)
                   res = db.TaiKhoans.Where(s => s.TenDangNhap == model.TenDangNhap).SingleOrDefault();
                else
                   res = null;
                if (res != null)
                {
                    if(res.TrangThai == 0)
                    {
                        _notyf.Error("Tài khoản chưa được kích hoạt! Kiểm tra email và đăng nhập lại sau");
                        return View();
                    }
                    _notyf.Success("Chào mừng bạn quay lại với UniShop!");
                    HttpContext.Session.SetInt32("Chan", 1);
                    if (model.Remember)
                    {
                        HttpContext.Session.SetString("remember", model.TenDangNhap);
                    }
                    if (res.MaQuyen == 1)
                    {
                        HttpContext.Session.SetInt32("Chan", 2);
                        HttpContext.Session.SetInt32("maTK", res.MaTaiKhoan);
                        HttpContext.Session.SetString("username", model.TenDangNhap);
                        HttpContext.Session.SetInt32("taikhoan", res.MaTaiKhoan);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    if (res.MaQuyen == 3)
                    {
                        HttpContext.Session.SetInt32("maTK", res.MaTaiKhoan);
                        HttpContext.Session.SetString("taikhoan", res.MaTaiKhoan.ToString());
                        HttpContext.Session.SetString("username", model.TenDangNhap);
                        return RedirectToAction("Filter1", "QuanLyNS", new { area = "Partner" });
                    }
                    HttpContext.Session.SetInt32("maTK", res.MaTaiKhoan);
                    var manguoidung = (from s in db.NguoiDungs where s.MaTaiKhoan == res.MaTaiKhoan select s.MaNguoiDung).FirstOrDefault();
                    HttpContext.Session.SetInt32("mand",manguoidung);
                    HttpContext.Session.SetString("username", model.TenDangNhap);
                    int count = db.GioHangs.Where(s => s.MaTaiKhoan == res.MaTaiKhoan).Count();
                    HttpContext.Session.SetInt32("countCart", count);
                    return RedirectPermanentPreserveMethod("/");
                }
                else
                {
                    
                    ModelState.AddModelError("", "Thông tin đăng nhập không đúng! ");
                }
            }
            else
            {
                ModelState.AddModelError("", "Thông tin đăng nhập nhập rỗng! ");
            }
            return View();
        }
        [Route("validate/{id:int}")]
        public async Task<IActionResult> validate([FromRoute] int id)
        {
            TaiKhoan res = db.TaiKhoans.Where(s => s.MaTaiKhoan == id).SingleOrDefault();
            res.TrangThai = 1;
            db.Update(res);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            TaiKhoan model = new TaiKhoan();
            return View(model);
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("TenDangNhap, MatKhau,Email")] TaiKhoan tk)
        {
            var existAcc = db.TaiKhoans.FirstOrDefault(p => p.TenDangNhap == tk.TenDangNhap);
            if (existAcc != null)
            {
                ViewBag.error = "Tên đăng nhập đã tồn tại! ";
                return View();
            }
            tk.MatKhau = Convert.ToBase64String(EncodeManager.HashPasswordV2(tk.MatKhau));
            tk.TrangThai = 0;
            tk.MaQuyen = 2;
            if (ModelState.IsValid)
            {
                db.Add(tk);
                await db.SaveChangesAsync();
                NguoiDung nd = new NguoiDung();
                nd.MaTaiKhoan = tk.MaTaiKhoan;
                HttpContext.Session.SetInt32("matktemp", tk.MaTaiKhoan);
                int id = HttpContext.Session.GetInt32("matktemp").Value;
                db.Add(nd);
                await db.SaveChangesAsync();
                var result = new EmailController().SendMail("UniShop-Validate Account", "validate_account", new string[1] { tk.Email}, id, null, null);
                _notyf.Success("Chào mừng bạn đến với UniShop! Vui lòng kiểm tra email và kích hoạt tài khoản nhé");
                return Redirect("/Login");
            }
            return View();
        }

        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("remember");
            HttpContext.Session.Remove("username");
            HttpContext.Session.Clear();

            //Response.Cookies.Delete("Khoa");
            Response.Cookies.Delete(".AspNetCore.Session");
            await HttpContext.SignOutAsync();
            return RedirectToAction("login");
        }

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }
        public const string CARTKEY = "cart";

        [Route("addcart/{productid:int}", Name = "addcart")]
        public async Task<IActionResult> AddToCart([FromRoute] int productid)
        {
            
            if (HttpContext.Session.GetInt32("maTK") == null)
            {
                return RedirectToAction("login");
            }
            var tk = (int)HttpContext.Session.GetInt32("maTK").Value;
            GioHang cartItem = (from s in db.GioHangs where s.MaNongSan == productid where s.MaTaiKhoan==tk select s).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Sl++;
                cartItem.MaNongSan = productid;
                cartItem.MaTaiKhoan = (int)HttpContext.Session.GetInt32("maTK").Value;
                if (ModelState.IsValid)
                {
                    db.Update(cartItem);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                cartItem = new GioHang();
                cartItem.Sl = 1;
                cartItem.MaNongSan = productid;
                cartItem.MaTaiKhoan = (int)HttpContext.Session.GetInt32("maTK").Value;
                if (ModelState.IsValid)
                {
                    db.Add(cartItem);
                    await db.SaveChangesAsync();

                }
                HttpContext.Session.Remove("countCart");
                int count = db.GioHangs.Where(s => s.MaTaiKhoan == cartItem.MaTaiKhoan).Count();
                HttpContext.Session.SetInt32("countCart", count);
            }
            return RedirectToAction("Index");
        }
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            var tk = (int)HttpContext.Session.GetInt32("maTK").Value;
            var cartItem = (from s in db.GioHangs where s.MaNongSan == productid where s.MaTaiKhoan==tk select s).FirstOrDefault();

            if (cartItem != null)
            {
                if (quantity == 0)
                {
                    db.Remove(cartItem);
                    await db.SaveChangesAsync();
                    HttpContext.Session.Remove("countCart");
                    int count = db.GioHangs.Where(s => s.MaTaiKhoan == cartItem.MaTaiKhoan).Count();
                    HttpContext.Session.SetInt32("countCart", count);
                }
                else
                {
                    cartItem.Sl = quantity;
                    db.Update(cartItem);
                    await db.SaveChangesAsync();
                }
            }
            else
                return NotFound();
            return RedirectToAction(nameof(Cart));
        }
        /*vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv*/
        [Route("checkout/{total:double}", Name = "checkout")]
        public IActionResult Checkout([FromRoute] double total)
        {
            HttpContext.Session.SetInt32("thanhtien", (Int32)total);
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login");
            }
            List<NongSan> nongsans = db.NongSans.ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<GioHang> giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
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
            NguoiDung nguoidungcontent = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
            var checkout = new CheckOut();
            checkout.cartcontentdetail = cartcontent;
            checkout.nguoidungdetail = nguoidungcontent;
            checkout.ThanhTien = total;

            return View(checkout);
        }
        /*vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv*/
       /* [Route("/addDonHang", Name = "addDonHang")]
        [HttpPost]
        public async Task<IActionResult> AddDonHang()
        {
            DonDat donhang = new DonDat();
            donhang.MaNguoiDung = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s.MaNguoiDung).FirstOrDefault();
            donhang.Ma_Trang_Thai = 1;
            donhang.NgayDatHang = DateTime.Now.ToString();
            db.Add(donhang);
            await db.SaveChangesAsync();

            var madondat = (from s in db.DonDats.OrderByDescending(x => x.MaDonDat) where s.MaNguoiDung == donhang.MaNguoiDung select s.MaDonDat).FirstOrDefault();
            List<GioHang> giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
            foreach (var element in giohangs)
            {
                ChiTietNsDd item = new ChiTietNsDd();
                item.MaDonDat = madondat;
                item.MaNongSan = element.MaNongSan;
                item.SoLuongDat = element.SL;
                db.Add(item);
                await db.SaveChangesAsync();
                db.Remove(element);
                await db.SaveChangesAsync();
               
            }
            int count = db.GioHangs.Where(s => s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK")).Count();
            HttpContext.Session.SetInt32("countCart", count);
            await db.SaveChangesAsync();
            ViewBag.Message = "Cập nhật đơn hàng thành công! ";

            return Redirect("/");
        }*/
        // Hiện thị giỏ hàng
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login");
            }
            List<NongSan> nongsans = db.NongSans.ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<GioHang> giohangs = (from s in db.GioHangs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).ToList();
            var cart = from g in giohangs
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
            return View(cart);
        }

        [Route("DonXinDT")]
        [HttpGet]
        public IActionResult Don_Xin_DT()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("login");
            }
            NguoiDung model = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
            HttpContext.Session.SetInt32("maND", model.MaNguoiDung);
            DonXinDT donxin = (from s in db.DonXinDTs where s.MaNguoiDung == (int)HttpContext.Session.GetInt32("maND").Value select s).FirstOrDefault();
            if (donxin != null)
            {
                return RedirectToAction("ExceptionError");
            }
            int? ktrDT = (from s in db.TaiKhoans where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s.MaQuyen).FirstOrDefault();
            if (ktrDT == 3)
            {
                return RedirectToAction("ExceptionError");
            }
            
            return View(model);
        }


        [HttpPost("DonXinDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DonXinDT(NguoiDung nd, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Lay ten luu vao bien fii
                    var fii = Path.GetFileName(myfile.FileName);
                    //Chi dinh duong dan se luu
                    string fullPAth = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/Users", myfile.FileName);

                    //copy file vao thu muc chi dinh
                    using (var file = new FileStream(fullPAth, FileMode.Create))
                    {
                        myfile.CopyTo(file);
                    }
                    nd.Avatar = fii;
                    nd.MaNguoiDung = (int)HttpContext.Session.GetInt32("maND").Value;
                    nd.MaTaiKhoan = (int)HttpContext.Session.GetInt32("maTK").Value;
                    DonXinDT dt = new DonXinDT();
                    dt.MaNguoiDung = (int)HttpContext.Session.GetInt32("maND").Value;
                    db.Update(nd);
                    db.Add(dt);                  
                    await db.SaveChangesAsync();
                    _notyf.Success("Yêu cầu của bạn đã được gửi thành công");

                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Vui long chon anh");
                    return RedirectToAction("DonXinDT");
                }
            }
            return RedirectToAction("index");
        }

        [Route("profile")]
        [HttpGet]
        public IActionResult Profile()
        {
            NguoiDung model = (from s in db.NguoiDungs where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s).FirstOrDefault();
            return View(model);
        }

        [HttpPost("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(NguoiDung nd, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Lay ten luu vao bien fii
                    var fii = Path.GetFileName(myfile.FileName);
                    //Chi dinh duong dan se luu
                    string fullPAth = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/Users", myfile.FileName);

                    //copy file vao thu muc chi dinh
                    using (var file = new FileStream(fullPAth, FileMode.Create))
                    {
                        myfile.CopyTo(file);
                    }
                    nd.Avatar = fii;
                    nd.MaNguoiDung = (int)HttpContext.Session.GetInt32("mand");
                    nd.MaTaiKhoan = (int)HttpContext.Session.GetInt32("maTK");
                    db.Update(nd);
                    await db.SaveChangesAsync();
                    _notyf.Success("Tài khoản cập nhật thành công");
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Vui long chon anh");
                    return RedirectToAction("profile");
                }
            }
            return RedirectToAction("index");
        }
        [Route("feedback")]
        [HttpGet]
        public IActionResult feedback()
        {
            return View();

        }

       /* [Route("/checkout")]
        public IActionResult CheckOut()
        {
            // Xử lý khi đặt hàng
            return View();
        }*/
        [Route("/error404")]
        public IActionResult ExceptionError(string message)
        {
            int donxin = (int)(from s in db.DonXinDTs where s.MaNguoiDung == (int)HttpContext.Session.GetInt32("maND").Value select s.MaNguoiDung).FirstOrDefault();
            if (donxin == (int)HttpContext.Session.GetInt32("maND"))
            {
                ViewBag.Message = "Đơn xin đối tác của bạn đang được xét duyệt.";
                return View();
            }
            int? ktrDT = (from s in db.TaiKhoans where s.MaTaiKhoan == (int)HttpContext.Session.GetInt32("maTK").Value select s.MaQuyen).FirstOrDefault();
            if (ktrDT == 6)
            {
                ViewBag.Message = "Chức năng này chỉ dành cho khách hàng.";
                return View();
            }
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //Dang nhap bang facebook
        public async Task SigninFacebook()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("Index")
            });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //Dang nhap bang google
        public async Task Signin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {

                RedirectUri = Url.Action("Index")
            });
        }
    }
}
