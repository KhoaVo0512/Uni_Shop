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
    public class NguoiDungController : Controller
    {
        TN230Context db = new TN230Context();
        public IActionResult Index(int pg)
        {
            if (HttpContext.Session.GetInt32("Chan") != 1)
            {
                int session = (int)HttpContext.Session.GetInt32("taikhoan");
                var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
                TempData["data"] = kh;
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            var nguoidung = (from s in db.NguoiDungs join b in db.TaiKhoans on s.MaTaiKhoan equals b.MaTaiKhoan where b.MaQuyen == 2 select s).ToList();
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = nguoidung.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nguoidung.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
    }
}
