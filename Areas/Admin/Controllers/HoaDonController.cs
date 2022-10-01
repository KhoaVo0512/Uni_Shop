using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;
using Microsoft.AspNetCore.Http;

namespace Uni_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonController : Controller
    {
        TN230Context db = new TN230Context();
        public IActionResult Index(int pg=1)
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
            List<Trang_Thai> trang_Thais = db.TrangThais.ToList();
            List<HoaDon> hoaDons = db.HoaDons.ToList();
            List<DonDat> donDats = db.DonDats.ToList();
            List<NguoiDung> nguoidungs = db.NguoiDungs.ToList();
            var display = from h in hoaDons
                          join d in donDats on h.MaDonDat 
                          equals d.MaDonDat into table1
                          from d in table1.DefaultIfEmpty()
                          join t in trang_Thais on d.Ma_Trang_Thai equals t.Ma_Trang_Thai into table3
                          from t in table3.DefaultIfEmpty()
                          join n in nguoidungs on d.MaNguoiDung equals n.MaNguoiDung into tables2
                          from n in tables2.DefaultIfEmpty()
                         
                          select new HoaDonCotent1
                          {
                              HoaDondetail = h,
                              DonDatdeltai = d,
                              NguoiDungdeltai = n,
                              trang_thai = t
                          };
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = display.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = display.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
    }
}
