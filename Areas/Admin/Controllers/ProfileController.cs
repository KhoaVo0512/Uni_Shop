using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;
namespace Uni_Shop.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProfileController : Controller
    {
        TN230Context db = new TN230Context();
        //[Authorize]
        public IActionResult Index()
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

            NhanVien nhanvien = nhanvien = db.NhanViens.SingleOrDefault(s => s.MaTaiKhoan== (int)HttpContext.Session.GetInt32("taikhoan"));
            return View(nhanvien);
        }

        [HttpGet]
        public IActionResult CapNhat(int? id)
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
            NhanVien nhanvien = db.NhanViens.SingleOrDefault(s => s.MaNhanVien == id);
            return View(nhanvien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhat(NhanVien nd, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //Lay ten luu vao bien fii
                    var fii = Path.GetFileName(myfile.FileName);
                    //Chi dinh duong dan se luu
                    string fullPAth = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin/assets/img/admin", myfile.FileName);

                    //copy file vao thu muc chi dinh
                    using (var file = new FileStream(fullPAth, FileMode.Create))
                    {
                        myfile.CopyTo(file);
                    }
                    nd.Avatar = fii;
                    nd.MaTaiKhoan = HttpContext.Session.GetInt32("taikhoan").Value;
                    db.Update(nd);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Vui long chon anh");
                    return RedirectToAction("CapNhat");
                }
            }
            return RedirectToAction("CapNhat");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {

            if (HttpContext.Session.GetInt32("Chan") != 1)
            {
                int session = (int)HttpContext.Session.GetInt32("taikhoan");
                var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
                TempData["data"] = kh;
                TempData["TenNV"] = (from s in db.NhanViens where s.MaTaiKhoan == session select s.TenNhanVien).Single();
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            int session = (int)HttpContext.Session.GetInt32("taikhoan");
            var kh = (from s in db.NhanViens where s.MaTaiKhoan == session select s.Avatar).Single();
            TempData["data"] = kh;
            if (ModelState.IsValid)
            {
                
                TempData["maNV"] = HttpContext.Session.GetInt32("maNV"); 
                ViewBag.maNV = HttpContext.Session.GetInt32("maNV");
                var tk = db.TaiKhoans.FirstOrDefault(u => u.MaTaiKhoan == HttpContext.Session.GetInt32("taikhoan"));
                tk.MatKhau = model.NewPassword;
                db.SaveChanges();
                ViewBag.khoa = true;
                return RedirectToAction("index");
            }
            return View(model);
        }
    }
}
