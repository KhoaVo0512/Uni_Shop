using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.Models;
using Uni_Shop.ModelDBs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uni_Shop.Areas.Partner.Controllers
{
    [Area("Partner")]
    public class DonDatController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();
        public IActionResult Index()
        {
            return View();
        }

        private void temp4(object selecttemp4 = null)
        {
            ViewBag.temp4 = new SelectList(db.NguoiDungs.ToList(), "MaNguoiDung", "TenNguoiDung", selecttemp4);
        }
        private void temp5(object selecttemp5 = null)
        {
            ViewBag.Compani = new SelectList(db.TrangThais.ToList(), "Ma_Trang_Thai", "Ten_Trang_Thai", selecttemp5);
        }
        public IActionResult dondat(int pg = 1)
        {
            //cach 1 dung list
            List<TaiKhoan> taikhoans = db.TaiKhoans.ToList();
            List<NguoiDung> nguoidungs = db.NguoiDungs.ToList();
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<DonDat> dondats = db.DonDats.ToList();
            List<Quyen> quyens = db.Quyens.ToList();
            List<NongSan> nongsans = db.NongSans.ToList();
            List<ChiTietNsDd> chitiets = db.ChiTietNsDds.ToList();
            List<Trang_Thai> trangthais = db.TrangThais.ToList();
            //Lay ID tai khoan goi ben logincontroller
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);

            temp5();
            temp4();
            if (dondats.Count != 0)
            {
                ViewBag.temp = 1;
                var dondat = from t in dondats
                             //join n in chitiets on t.MaDonDat equals n.MaDonDat into table1
                             //from n in table1.DefaultIfEmpty()
                             //join g in nongsans on n.MaNongSan equals g.MaNongSan into table2
                             //from g in table2.DefaultIfEmpty()
                             //join d in gianhangs on g.MaGianHang equals d.MaGianHang into table3
                             //from d in table3.DefaultIfEmpty()
                             //join e in nguoidungs on d.MaNguoiDung equals e.MaNguoiDung into table4
                             //from e in table4.DefaultIfEmpty()
                             //join f in taikhoans on e.MaTaiKhoan equals f.MaTaiKhoan into table5
                             //from f in table5.DefaultIfEmpty()
                             join a in trangthais on t.MaTrangThai equals a.Ma_Trang_Thai
                             select new ghdd
                             {
                                 dondatdetail = t,
                                 //chitietdetail = n,
                                 //nongsandetail = g,
                                 //nguoidungdetail = e,
                                 trangthaidetail = a
                             };

                const int pageSize = 3;
                if (pg < 1)
                    pg = 1;
                int recsCount = dondat.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = dondat.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(data);
            }
            else
            {
                ViewBag.temp = 0;
                return View();
            }
           
        }


        [HttpPost]
        [Route("/Partner/DonDat/EditDD", Name = "EditDD")]
        public async Task<IActionResult> EditDD([FromForm] int madondat, [FromForm] int trangthaiid)
        {
            var DDitem = (from s in db.DonDats where s.MaDonDat == madondat select s).FirstOrDefault();
            if (DDitem != null)
            {
                DDitem.MaTrangThai = trangthaiid;
                db.Update(DDitem);
                await db.SaveChangesAsync();
                if (trangthaiid == 4)
                {
                    HoaDon a = new HoaDon();
                    DateTime now = DateTime.Now;
                    //    //c1 hien khong gio
                    DateTime c = new DateTime(now.Year, now.Month, now.Day);
                    a.NgayGiaoHang = now.Day + "-" + now.Month + "-" + now.Year;
                    //    //c2 hien co gio
                    //a.NgayGiaoHang = now.ToString();    //hien c2 bo hang nay
                    //int b = dondats.MaDonDat;
                    int b = DDitem.MaDonDat;

                    a.MaDonDat = b;
                    db.HoaDons.Add(a);
                    db.SaveChanges();
                }
            }
            else
                return NotFound();
            //return RedirectToAction(nameof(dondat));
            //return RedirectToAction("Origingiaodiendoitac");
            return RedirectToAction("giaodiendoitac", "Home", new { area = "Partner" });
        }
    }
}
