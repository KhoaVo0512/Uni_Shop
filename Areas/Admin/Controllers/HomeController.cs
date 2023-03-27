using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Chan") != 1)
            {
                int session = (int)HttpContext.Session.GetInt32("taikhoan");
                var kh1 = db.NhanViens.Where(s=> s.MaTaiKhoan==session);
                if (kh1 == null)
                {
                    TempData["data"] = "";
                }
                else
                {
                    var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
                    TempData["data"] = kh;
                }
                   
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            ViewBag.countKH = (from s in db.TaiKhoans where s.MaQuyen == 2 select s).ToList().Count();
            ViewBag.countloaisp = (from s in db.LoaiNongSans select s).ToList().Count();
            ViewBag.countdoitac = (from s in db.TaiKhoans where s.MaQuyen == 3 select s).ToList().Count();
            return View();
        }
        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("remember");
            HttpContext.Session.Remove("username");
            HttpContext.Session.Clear();

            //Response.Cookies.Delete("Khoa");
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("login");
        }
    }
}
