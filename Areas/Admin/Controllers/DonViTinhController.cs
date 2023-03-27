
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonViTinhController : Controller
    {

        TN230_V1Context db = new TN230_V1Context();

        public IActionResult Index(int pg = 1)
        {
            var session = HttpContext.Session.GetInt32("taikhoan");
            var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
            TempData["data"] = kh;
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = donViTinhs.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = donViTinhs.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonViTinh donvitinh)
        {
            int session = (int)HttpContext.Session.GetInt32("taikhoan");
            var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
            TempData["data"] = kh;
            if (ModelState.IsValid)
            {
                try
                {
                    db.DonViTinhs.Add(donvitinh);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Loi");
                    return View(donvitinh);
                }
            }
            return View(donvitinh);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            int session = (int)HttpContext.Session.GetInt32("taikhoan");
            var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
            TempData["data"] = kh;
            if (id == null)
            {
                return BadRequest();
            }

            var donvitinh = db.DonViTinhs.Find(id);
            if (donvitinh == null)
            {
                return NotFound();
            }

            return View(donvitinh);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var donvitinh = db.DonViTinhs.FirstOrDefault(s => s.MaDonViTinh == id);
            if (await TryUpdateModelAsync<DonViTinh>(donvitinh, "", s => s.TenDonViTinh))
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
            return View(donvitinh);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var donvitinh = db.DonViTinhs.Find(id);
            if (donvitinh == null)
            {
                return NotFound();
            }

            return View(donvitinh);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var donvitinh = db.DonViTinhs.Find(id);
            if (donvitinh == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                db.DonViTinhs.Remove(donvitinh);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException )
            {   
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
