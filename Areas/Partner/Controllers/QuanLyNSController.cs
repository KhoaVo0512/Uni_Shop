using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Uni_Shop.Models;
namespace Uni_Shop.Areas.Partner.Controllers
{
    [Area("Partner")]
    public class QuanLyNSController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();
        private readonly INotyfService _notyf;
        public IActionResult Index()
        {
            return View();
        }
        public INotyfService thongbao { get; }
        public QuanLyNSController(INotyfService notyfService, INotyfService notyf)
        {
            thongbao = notyfService;
            _notyf = notyf;
        }


        public IActionResult Filter1(int pg = 1)
        {
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);

            List<LoaiNongSan> loainongsan = db.LoaiNongSans.ToList();
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = (db.NongSans.Where(s => s.TrangThai == 1)).ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            List<NguoiDung> nguoiDungs = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).ToList();


            int nguoiDungs1 = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().MaNguoiDung ;
            int gianhangnew = db.GianHangs.Where(s => s.MaNguoiDung == nguoiDungs1).Count();
            if(gianhangnew == 0)
            {
                return RedirectToAction("CreatePartnerNew", "QuanLyNS", new { area = "Partner" });
            }


            string tennguoidung = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().TenNguoiDung;
            ViewBag.tennguoidung = tennguoidung;
            string tengianhan = db.GianHangs.Where(s => s.MaNguoiDung == nguoiDungs1).SingleOrDefault().TenGianHang;
            ViewBag.tengianhang = tengianhan;


            var nongsan = from nd in nguoiDungs
                          join g in gianhangs on nd.MaNguoiDung equals g.MaNguoiDung into table1
                          from g in table1.DefaultIfEmpty()
                          join n in nongsans on g.MaGianHang equals n.MaGianHang into table2
                          from n in table2.DefaultIfEmpty()
                          join a in anhs on n.MaNongSan equals a.MaNongSan into table3
                          from a in table3.DefaultIfEmpty()
                          join l in loainongsans on n.MaLoaiNongSan equals l.MaLoaiNongSan into table4
                          from l in table4.DefaultIfEmpty()
                          join d in donViTinhs on n.MaDonViTinh equals d.MaDonViTinh
                          where n.MaLoaiNongSan == 1
                          select new NongSanContent
                          {
                              nongsandetail = n,
                              gianhangdetail = g,
                              loainongsandeltail = l,
                              donvitinhdetail = d,
                              nguoidungdetail = nd,
                              anhnongsandeltail = a
                          };
            const int pageSize = 8;
            if (pg < 1)
                pg = 1;
            int recsCount = nongsan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nongsan.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.nongsan = data;
            return View(loainongsan);
        }
        [Route("/Partner/QuanLyNS/list1/{maloai}", Name = "ShopList1")]
        public IActionResult ShopList1(int maloai, int pg = 1)
        {
            int i = 1;
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);

            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = db.NongSans.Where(x => x.TrangThai == i).ToList();
            List<AnhN> anhs = db.AnhNs.ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            List<NguoiDung> nguoiDungs = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).ToList();
        
                    var nongsan = from nd in nguoiDungs
                          join g in gianhangs on nd.MaNguoiDung equals g.MaNguoiDung into table1
                          from g in table1.DefaultIfEmpty()
                          join n in nongsans on g.MaGianHang equals n.MaGianHang into table2
                          from n in table2.DefaultIfEmpty()
                          join a in anhs on n.MaNongSan equals a.MaNongSan into table3
                          from a in table3.DefaultIfEmpty()
                          join l in loainongsans on n.MaLoaiNongSan equals l.MaLoaiNongSan into table4
                          from l in table4.DefaultIfEmpty()
                          join d in donViTinhs on n.MaDonViTinh equals d.MaDonViTinh
                          where n.MaLoaiNongSan == maloai
                          select new NongSanContent
                          {
                              nongsandetail = n,
                              gianhangdetail = g,
                              loainongsandeltail = l,
                              donvitinhdetail = d,
                              anhnongsandeltail = a
                          };
           
            
            const int pageSize = 8;
            if (pg < 1)
                pg = 1;
            int recsCount = nongsan.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = nongsan.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.nongsan = data;
            return View(nongsan);
        }


        private void temp1(object selecttemp1 = null)
        {
            ViewBag.temp1 = new SelectList(db.GianHangs.ToList(), "MaGianHang", "TenGianHang", selecttemp1);
        }
        private void temp2(object selecttemp2 = null)
        {
            ViewBag.temp2 = new SelectList(db.LoaiNongSans.ToList(), "MaLoaiNongSan", "TenLoaiNongSan", selecttemp2);
        }
        private void temp4(int id, object selecttemp2 = null)
        {
            ViewBag.temp4 = new SelectList(db.LoaiNongSans.Where(s => s.MaLoaiNongSan == id).ToList(), "MaLoaiNongSan", "TenLoaiNongSan", selecttemp2);
        }
        private void temp3(object selecttemp3 = null)
        {
            ViewBag.temp3 = new SelectList(db.DonViTinhs.ToList(), "MaDonViTinh", "TenDonViTinh", selecttemp3);
        }
        [Route("/Partner/QuanLyNS/create1/{id2}", Name = "CreateNS")]
        public IActionResult CreateNS(int id2)
        {
            temp1();
            temp4(id2);
            temp3();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUploadFile(CreateNSIMG sanpham1, IFormFile myfile)
        {
           
                try
                {
                    //Lay ten luu vao bien fii
                    var fii = Path.GetFileName(myfile.FileName);
                    //Chi dinh duong dan se luu
                    string fullPAth = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MyFiles", myfile.FileName);

                    //copy file vao thu muc chi dinh
                    using (var file = new FileStream(fullPAth, FileMode.Create))
                    {
                        myfile.CopyTo(file);
                    }
                    int d = 1;
                    NongSan a = new NongSan();
                    a.MaLoaiNongSan = sanpham1.nongsandetail.MaLoaiNongSan;
                    a.MaGianHang = sanpham1.gianhangdetail.MaGianHang;
                    a.TrangThai = d;
                    a.MaDonViTinh = sanpham1.donvitinhdetail.MaDonViTinh;
                    a.TenNongSan = sanpham1.nongsandetail.TenNongSan;
                    a.TrongLuong = sanpham1.nongsandetail.TrongLuong;
                    a.SoLuong = sanpham1.nongsandetail.SoLuong;
                    a.Gia = sanpham1.nongsandetail.Gia;
                    a.MoTa = sanpham1.nongsandetail.MoTa;
                    db.NongSans.Add(a);
                    await db.SaveChangesAsync();
                    AnhN b = new AnhN();
                    b.MaNongSan = a.MaNongSan;
                    b.LinkAnh = fii;
                    b.MoTa = "Chưa cập nhật";
                    //  nongSan.Link_Anh = fii;
                    //  db.AnhNs.Add(b);
                    db.AnhNs.Add(b);
                    await db.SaveChangesAsync();
                    thongbao.Success("Thêm sản phẩm thành công");
                    return RedirectToAction(nameof(Filter1));

                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Vui long chon anh");
                    return View(sanpham1);
                }
            }




        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{

        //    var sanpham1 = db.NongSans.Find(id);
        //    if (sanpham1 == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(sanpham1);
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sp = db.NongSans.Find(id);
            if (sp == null)
            {
                return RedirectToAction(nameof(Filter1));
            }

            try
            {
                sp.TrangThai = 0;
                db.NongSans.Update(sp);
                db.SaveChanges();
                _notyf.Success("Xóa sản phẩm thành công");
                return RedirectToAction(nameof(Filter1));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }



        public IActionResult CreatePartnerNew()
        {
            temp2();
            temp3();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUploadPartner(CreateImg nongSan, IFormFile myfile)
        {
            
                try
                {
                    //Lay ten luu vao bien fii
                    var fii = Path.GetFileName(myfile.FileName);
                    //Chi dinh duong dan se luu
                    string fullPAth = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MyFiles", myfile.FileName);

                    //copy file vao thu muc chi dinh
                    using (var file = new FileStream(fullPAth, FileMode.Create))
                    {
                        myfile.CopyTo(file);
                    }
                    string id = HttpContext.Session.GetString("taikhoan");
                    int ma = Convert.ToInt32(id);

                    int manguoidung = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().MaNguoiDung;
                    GianHang c = new GianHang();
                    c.MaNguoiDung = manguoidung;
                    c.TenGianHang = nongSan.gianhangdetail.TenGianHang;
                    db.GianHangs.Add(c);
                    await db.SaveChangesAsync();



                    int d = 1;
                    NongSan a = new NongSan();
                    a.MaLoaiNongSan = nongSan.nongsandetail.MaLoaiNongSan;
                    a.MaGianHang = c.MaGianHang;
                    a.TrangThai = d;
                    a.MaDonViTinh = nongSan.donvitinhdetail.MaDonViTinh;
                    a.TenNongSan = nongSan.nongsandetail.TenNongSan;
                    a.TrongLuong = nongSan.nongsandetail.TrongLuong;
                    a.SoLuong = nongSan.nongsandetail.SoLuong;
                    a.Gia = nongSan.nongsandetail.Gia;
                    a.MoTa = nongSan.nongsandetail.MoTa;
                    db.NongSans.Add(a);
                    await db.SaveChangesAsync();



                    AnhN b = new AnhN();
                    b.MaNongSan = a.MaNongSan;
                    b.LinkAnh = fii;
                    //          nongSan.Link_Anh = fii;
                    db.AnhNs.Add(b);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Filter1));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Vui long chon anh");
                    return View(nongSan);
                }
            
        }


    }
}
