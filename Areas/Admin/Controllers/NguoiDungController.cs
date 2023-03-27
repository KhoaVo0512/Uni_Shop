using Antlr.Runtime.Misc;
using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uni_Shop.Controllers;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;

namespace Uni_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NguoiDungController : Controller
    {
        private readonly INotyfService _notyf;
        TN230_V1Context db = new TN230_V1Context();
        public NguoiDungController(INotyfService notyf)
        {
            _notyf = notyf;
        }
        public IActionResult Index(int pg, string SearchText="")
        {
            if (HttpContext.Session.GetInt32("Chan") != 1)
            {
                int session = (int)HttpContext.Session.GetInt32("taikhoan");
                var kh1 = db.NhanViens.Where(s => s.MaTaiKhoan == session);
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

            List<NguoiDung> nguoiDungs = db.NguoiDungs.ToList();
            List<TaiKhoan> taiKhoans = db.TaiKhoans.ToList();
            List<NguoiDungContext> nguoidung;
            if (SearchText != "" && SearchText != null)
            {
                nguoidung = (from s in nguoiDungs
                                 join b in taiKhoans
                           on s.MaTaiKhoan equals b.MaTaiKhoan
                                 where b.MaQuyen == 2
                                 where b.TenDangNhap.Contains(SearchText)
                                 select new NguoiDungContext
                                 {
                                     NguoiDung = s,
                                     TaiKhoan = b
                                 }).ToList();
            }
            else
            {
                 nguoidung = (from s in nguoiDungs join b in taiKhoans 
                                            on s.MaTaiKhoan equals b.MaTaiKhoan where b.MaQuyen == 2
                                            select new NguoiDungContext
                                            {
                                                NguoiDung = s,
                                                TaiKhoan = b
                                            }).ToList();
            }
               
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
        public async void Duyet(int id)
        {
            var tk = db.TaiKhoans.Where(s=>s.MaTaiKhoan==id).FirstOrDefault();
            try
            {
                tk.MaQuyen = 1;
                var ng = db.NguoiDungs.Where(s => s.MaTaiKhoan == id).FirstOrDefault();
                NhanVien nv = new NhanVien();
                nv.MaTaiKhoan = ng.MaTaiKhoan;
                nv.Avatar = ng.Avatar;
                nv.Sdt = ng.Sdt;
                nv.DiaChi = ng.DiaChi;
                nv.TenNhanVien = ng.TenNguoiDung;
                db.Add(nv); 
                _notyf.Success("Tài khoản của bạn đã lên quyền admin");
                await db.SaveChangesAsync();
               
                /*return RedirectToAction(nameof(Index));*/
            }
            catch (DbUpdateException)
            {
                //return RedirectToAction(nameof(Duyet), new { id = id, saveChangesError = true });
            }
        }
        public IActionResult KhuyenMai()
        {
            List<TaiKhoan> tk = db.TaiKhoans.ToList();
            int count = tk.Count;
            int i = 0;
            string[] listEmail = new string[count];
            foreach (var item in tk)
            {
                if (listEmail.Contains(item.Email) == false)
                {
                    listEmail[i] = item.Email;
                    i++;
                }
            }
            Console.WriteLine(listEmail);
            var result = new EmailController().SendMail("UniShop khuyến mãi lên đến 50%", "advertise_email", listEmail, null, null, null);
            _notyf.Success("Gửi mail khuyến mãi thành công!");
            //var result = new EmailController().SendMail("UniShop-Validate Account", "validate_account", new string[1] { "mylinhpthi@gmail.com" }, 1029);
            return RedirectToAction("Index", "NguoiDung");
        }
    }
}
