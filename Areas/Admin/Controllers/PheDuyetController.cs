using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;

namespace Uni_Shop.Areas.Admin.Controllers
{
    public class PheDuyetController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();
        public IActionResult Index()
        {
            List<NhanVien> nhanviens = db.NhanViens.ToList();
            List<TaiKhoan> taikhoans = db.TaiKhoans.ToList();
            var index = from t in taikhoans
                        join n in nhanviens on t.MaTaiKhoan equals n.MaTaiKhoan
                        where t.MaQuyen == 1
                        select new DonXinContext
                        {
                            taikhoan = t,
                            NhanVien = n
                        };
            return View(index);
        }
    }
}
