using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiNongSanController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();

        public IActionResult Index(int pg = 1, string SearchText="")
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
            List<LoaiNongSan> loaiNongSans;
            if (SearchText !=null && SearchText != "")
            {
                loaiNongSans = (from s in db.LoaiNongSans where s.TenLoaiNongSan.Contains(SearchText) select s).ToList();
            }
            else
            {
                loaiNongSans = db.LoaiNongSans.ToList();
            }
           
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = loaiNongSans.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = loaiNongSans.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        public ActionResult Create()
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
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoaiNongSan loainongsan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.LoaiNongSans.Add(loainongsan);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Loi");
                    return View(loainongsan);
                }
            }
            return View(loainongsan);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
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

            var loainongsan = db.LoaiNongSans.Find(id);
            if (loainongsan == null)
            {
                return NotFound();
            }

            return View(loainongsan);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var loainongsan = db.LoaiNongSans.FirstOrDefault(s => s.MaLoaiNongSan == id);
            if (await TryUpdateModelAsync<LoaiNongSan>(loainongsan, "", s => s.TenLoaiNongSan))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(loainongsan);
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

            var loainongsan = db.LoaiNongSans.Find(id);
            if (loainongsan == null)
            {
                return NotFound();
            }

            return View(loainongsan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var loainongsan = db.LoaiNongSans.Find(id);
            if (loainongsan == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.LoaiNongSans.Remove(loainongsan);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
