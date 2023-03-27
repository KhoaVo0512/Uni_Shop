using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;
namespace Uni_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoiTacController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();

        public IActionResult Index(int pg = 1, string SearchText = "")
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
            IEnumerable<DoiTacContent> doitac;
            if (SearchText != null && SearchText != "")
            {
                doitac = (from s in db.NguoiDungs
                          join b in db.TaiKhoans on s.MaTaiKhoan equals b.MaTaiKhoan into table1
                          from b in table1
                          join g in db.GianHangs on s.MaNguoiDung equals g.MaNguoiDung
                          where b.MaQuyen == 3
                          where g.TenGianHang.Contains(SearchText)
                          select new DoiTacContent
                          {
                              nguoidung = s,
                              gianHang = g
                          });
            }
            else
            {
                doitac = (from s in db.NguoiDungs
                          join b in db.TaiKhoans on s.MaTaiKhoan equals b.MaTaiKhoan into table1
                          from b in table1
                          join g in db.GianHangs on s.MaNguoiDung equals g.MaNguoiDung
                          where b.MaQuyen == 3
                          select new DoiTacContent
                          {
                              nguoidung = s,
                              gianHang = g
                          });

            }

            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = doitac.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = doitac.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
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
            if (id == null)
            {
                return BadRequest();
            }

            var nguoidung = db.NguoiDungs.Find(id);
            if (nguoidung == null)
            {
                return NotFound();
            }

            return View(nguoidung);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var nd = db.NguoiDungs.Find(id).MaTaiKhoan;
            var tk = db.TaiKhoans.FirstOrDefault(u => u.MaTaiKhoan == nd);
            tk.MaQuyen = 2;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult XetDuyetDT(int pg = 1)
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
            List<DonXinDT> donXinDTs = db.DonXinDTs.Where(s=>s.NgayDuyet == null).ToList();
            List<NguoiDung> nguoiDungs = db.NguoiDungs.ToList();
            var nd = from d in donXinDTs
                     join n in nguoiDungs on d.MaNguoiDung equals n.MaNguoiDung
                     select new DoiTacContent
                     {
                         donxinDT = d,
                         nguoidung = n
                     };
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = nd.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nd.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        public IActionResult Yes(int? id)
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
            if (id == null)
            {
                return BadRequest();
            }

            var nguoidung = db.NguoiDungs.Find(id);
            if (nguoidung == null)
            {
                return NotFound();
            }

            return View(nguoidung);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Yes(int id)
        {
            var nd = db.NguoiDungs.Find(id).MaTaiKhoan;
            var tk = db.TaiKhoans.FirstOrDefault(u => u.MaTaiKhoan == nd);
            tk.MaQuyen = 3;
            var dt = db.DonXinDTs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            dt.NgayDuyet = DateTime.Today;
            db.SaveChanges();
            return RedirectToAction("XetDuyetDT");
        }
        [HttpGet]
        public IActionResult No(int? id)
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
            if (id == null)
            {
                return BadRequest();
            }

            var nguoidung = db.NguoiDungs.Find(id);
            if (nguoidung == null)
            {
                return NotFound();
            }

            return View(nguoidung);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult No(int id)
        {
            var nd = db.DonXinDTs.Where(s => s.MaNguoiDung == id).SingleOrDefault();
            db.DonXinDTs.Remove(nd);
            db.SaveChanges();
            return RedirectToAction("XetDuyetDT");
        }
    }
}
