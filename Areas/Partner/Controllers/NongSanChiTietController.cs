using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.Models;
using Uni_Shop.ModelDBs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uni_Shop.Areas.Partner.Controllers
{
    [Area("Partner")]
    public class NongSanChiTietController : Controller
    {
        TN230Context db = new TN230Context();

        public IActionResult Index()
        {
            return View();
        }
        private void temp1(object selecttemp1 = null)
        {
            ViewBag.temp1 = new SelectList(db.LoaiNongSans.ToList(), "MaLoaiNongSan", "TenLoaiNongSan", selecttemp1);
        }
        private void temp2(object selecttemp2 = null)
        {
            ViewBag.temp2 = new SelectList(db.DonViTinhs.ToList(), "MaDonViTinh", "TenDonViTinh", selecttemp2);
        }
        private void temp3(object selecttemp3 = null)
        {
            ViewBag.temp3 = new SelectList(db.GianHangs.ToList(), "MaGianHang", "TenGianHang", selecttemp3);
        }
        public IActionResult NongSanChiTietDoiTac1(int id)
        {
            temp1();
            temp2();
            temp3();
            return View(db.NongSans.Find(id));
        }
    }
}
