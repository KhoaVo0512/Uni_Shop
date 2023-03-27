using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Uni_Shop.Models;

namespace Uni_Shop.Areas.Partner.Controllers
{
    [Area("Partner")]
    public class HomeController : Controller
    {
        TN230_V1Context db = new TN230_V1Context();
        public IActionResult Index(int pg = 1)
        {
            int f = 1;
            List<NongSan> nongsans = (db.NongSans.Where(s => s.TrangThai == f)).ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<AnhN> anhns = db.AnhNs.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            List<GianHang> gianHangs = db.GianHangs.ToList();
            var index = from t in nongsans
                        join n in anhns on t.MaNongSan equals n.MaNongSan into table1
                        from n in table1.DefaultIfEmpty()
                        join g in gianHangs on t.MaGianHang equals g.MaGianHang into table2
                        from g in table2.DefaultIfEmpty()
                        join l in loainongsans on t.MaLoaiNongSan equals l.MaLoaiNongSan into table3
                        from l in table3.DefaultIfEmpty()
                        join d in donViTinhs on t.MaDonViTinh equals d.MaDonViTinh
                        select new NongSanChiTiet
                        {
                            nongsandetail = t,
                            anhnsdetail = n,
                            gianhangdetail = g,
                            loainongsandeltail = l,
                            donvitinhdetail = d
                        };

            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int recsCount = index.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = index.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }

        public IActionResult gianhang(int pg = 1)
        {
            int a = 1;
        
            //cach 1 dung list
            List<TaiKhoan> taikhoans = db.TaiKhoans.ToList();
            List<NguoiDung> nguoidungs = db.NguoiDungs.ToList();
            List<GianHang> gianhangs = db.GianHangs.ToList();
            List<NongSan> nongsans = (db.NongSans.Where(s => s.TrangThai == a)).ToList();
            List<LoaiNongSan> loainongsans = db.LoaiNongSans.ToList();
            List<DonViTinh> donViTinhs = db.DonViTinhs.ToList();
            List<AnhN> anhns = db.AnhNs.ToList();
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);
            var gh =(from s in db.NguoiDungs where s.MaTaiKhoan == Convert.ToInt32(id) select s.MaNguoiDung).Single();
            var gh1 = db.GianHangs.Where(s => s.MaNguoiDung ==gh).SingleOrDefault();
            if (gh1 == null)
            {
                GianHang gianHang = new GianHang();
                gianHang.MaNguoiDung = gh;
                gianHang.TenGianHang = "Vui long nhập tên gian hàng";
                db.Add(gianHang);
                db.SaveChanges();
                //ViewBag.temp = 0;
                //List<GianHang> gianhangst = (from s in db.GianHangs where s.MaGianHang == gianHang.MaGianHang).ToList();
                //var nongsan = from g in gianhangst
                //              join d in nguoidungs on g.MaNguoiDung equals d.MaNguoiDung into table2
                //              from d in table2.DefaultIfEmpty()
                //              join e in taikhoans on d.MaTaiKhoan equals e.MaTaiKhoan into table3
                //              from e in table3.DefaultIfEmpty()
                //              where e.MaTaiKhoan == ma
                //              select new NongSanContentP
                //              {
                //                  nguoidungdetail = d,
                //                  taikhoandetail = e,
                //                  gianhangdetail = g

                //              };
                //return View(nongsan);
                return RedirectToAction(nameof(Create));

            }
            if (nongsans.Count != 0)
            {
                ViewBag.temp = 1;

                var nongsan = from g in gianhangs
                              join n in nongsans on g.MaGianHang equals n.MaGianHang into table1
                              from n in table1.DefaultIfEmpty()
                              join d in nguoidungs on g.MaNguoiDung equals d.MaNguoiDung into table2
                              from d in table2.DefaultIfEmpty()
                              join e in taikhoans on d.MaTaiKhoan equals e.MaTaiKhoan into table3
                              from e in table3.DefaultIfEmpty()
                              join f in loainongsans on n.MaLoaiNongSan equals f.MaLoaiNongSan into table4
                              from f in table4.DefaultIfEmpty()
                              join h in donViTinhs on n.MaDonViTinh equals h.MaDonViTinh into table5
                              from h in table5.DefaultIfEmpty()
                              join y in anhns on n.MaNongSan equals y.MaNongSan
                              where e.MaTaiKhoan == ma
                              select new NongSanContentP
                              {
                                  nguoidungdetail = d,
                                  taikhoandetail = e,
                                  nongsandetail = n,
                                  gianhangdetail = g,
                                  loainongsandeltail = f,
                                  donvitinhdetail = h,
                                  anhdetail = y
                              };
                const int pageSize = 6;
                if (pg < 1)
                    pg = 1;
                int recsCount = nongsan.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = nongsan.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(data);
            }
            return RedirectToAction(nameof(gianhang));

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
        [HttpPost]
        public async Task<IActionResult> EditGH([FromForm] int magianhang, [FromForm] string tengianhang)
        {
            var DDitem = (from s in db.GianHangs where s.MaGianHang == magianhang select s).FirstOrDefault();
            if (DDitem != null)
            {
                DDitem.TenGianHang = tengianhang;
                db.Update(DDitem);
                await db.SaveChangesAsync();
            }
            else
                return NotFound();
            return RedirectToAction(nameof(gianhang));
        }

        public IActionResult Create()
        {
            temp1();
            temp2();
            temp3();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUploadFile(CreateImg nongSan, IFormFile myfile)
        {
            //if (ModelState.IsValid)
            //{
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
                    a.MaLoaiNongSan = nongSan.nongsandetail.MaLoaiNongSan;
                    a.MaGianHang = nongSan.gianhangdetail.MaGianHang;
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

                    return RedirectToAction(nameof(gianhang));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Vui long chon anh");
                    return View(nongSan);
                }
            }
        //    return View(nongSan);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUpLoadImg(NongSan nongSan)
        {
            //if (ModelState.IsValid)
            //{
                
                nongSan.TrangThai = 1;
                db.NongSans.Update(nongSan);
                await db.SaveChangesAsync();
                return RedirectToAction("Filter1", "QuanLyNS", new { area = "Partner" });
            //}
            //return View(nongSan);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var nongsan = db.NongSans.Find(id);
            if (nongsan == null)
            {
                return NotFound();
            }
            return View(nongsan);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var nongsan = db.NongSans.Find(id);
            if (nongsan == null)
            {
                return RedirectToAction(nameof(Index));
            }
            nongsan.TrangThai = 0;
            db.NongSans.Update(nongsan);
            db.SaveChanges();
            return RedirectToAction(nameof(gianhang));
        }

        private void temp5(object selecttemp3 = null)
        {
            ViewBag.temp5 = new SelectList(db.NongSans.ToList(), "MaNongSan", "TenNongSan", selecttemp3);
        }
        [HttpPost]
        public IActionResult EditImage(int? maimg)
        {
            temp5();
            if (maimg == null)
            {
                return BadRequest();
            }

            List<AnhN> anhNs = db.AnhNs.Where(s => s.MaNongSan == maimg).ToList();
            ViewBag.img = anhNs[0].LinkAnh;
            int ma = anhNs[0].MaAnh;
            var img = db.AnhNs.Find(ma);

            if (img == null)
            {
                return NotFound();
            }
            return View(img);
        }

        [HttpPost, ActionName("Updateimg")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Updateimg(AnhN id, IFormFile myfile)
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
            AnhN a = new AnhN();
            a.MaAnh = id.MaAnh;
            a.MaNongSan = id.MaNongSan;
            a.LinkAnh = fii;
            db.AnhNs.Update(a);
            await db.SaveChangesAsync();
            return RedirectToAction("Filter1", "QuanLyNS", new { area = "Partner" });
        }

        
        private void temp7(object selecttemp5 = null)
        {
            ViewBag.Compani1 = new SelectList(db.TrangThais.ToList(), "Ma_Trang_Thai", "Ten_Trang_Thai", selecttemp5);

            

        }

        [HttpGet]
        public IActionResult giaodiendoitac(int pg = 1)
        {

            List<Trang_Thai> trang_Thais = db.TrangThais.ToList();
            List<NongSan> nongSans = db.NongSans.ToList();
            List<GianHang> gianHangs = db.GianHangs.ToList();
            List<DonDat> donDats = db.DonDats.Where(x => x.MaTrangThai == 1).ToList();
            List<NguoiDung> nguoiDungs = db.NguoiDungs.ToList();
            List<ChiTietNsDd> chiTietNsDds = db.ChiTietNsDds.ToList();
            List<HinhThucThanhToan> hinhThucThanhToans = db.HinhThucThanhToans.ToList();
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);

            int nguoiDungs1 = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().MaNguoiDung;
            string tennguoidung = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().TenNguoiDung;
            ViewBag.tennguoidung = tennguoidung;
            string tengianhan = db.GianHangs.Where(s => s.MaNguoiDung == nguoiDungs1).SingleOrDefault().TenGianHang;
            ViewBag.tengianhang = tengianhan;

            temp7();



            //int matk = db.NguoiDungs.Where(s => s.MaTaiKhoan == ma).SingleOrDefault().MaNguoiDung;
            //int gianhangnew = db.GianHangs.Where(s => s.MaNguoiDung == matk).SingleOrDefault().MaGianHang;


            //var doitac = (from d in donDats
            //              join c in chiTietNsDds on d.MaDonDat equals c.MaDonDat into table1
            //              from c in table1.DefaultIfEmpty()
            //              join n in nongSans on c.MaNongSan equals n.MaNongSan into table2
            //              from n in table2.DefaultIfEmpty()
            //              join hh in hinhThucThanhToans on d.MaHttt equals hh.MaHtth                          
            //              where  n.MaGianHang == gianhangnew
            //              select new PartnerContent
            //              {

            //                  chitietdetail = c,
            //                  nongsandetail = n,

            //                  hinhthucdetail = hh,
            //                  dondatdetail = d

            //              });

            var doitac = (from d in donDats
                          join c in chiTietNsDds on d.MaDonDat equals c.MaDonDat into table1
                          from c in table1.DefaultIfEmpty()
                          join n in nongSans on c.MaNongSan equals n.MaNongSan into table2
                          from n in table2.DefaultIfEmpty()
                          join g in gianHangs on n.MaGianHang equals g.MaGianHang into table3
                          from g in table3.DefaultIfEmpty()
                          join nd in nguoiDungs on g.MaNguoiDung equals nd.MaNguoiDung into table4
                          from nd in table4.DefaultIfEmpty()
                          join hh in hinhThucThanhToans on d.MaHttt equals hh.MaHtth
                          where nd.MaTaiKhoan == ma && n.TrangThai == 1
                          select new PartnerContent
                          {

                              chitietdetail = c,
                              nongsandetail = n,
                              gianhangdetail = g,
                              nguoidungdetail = nd,
                              hinhthucdetail = hh,
                              dondatdetail = d
                              

                          });
            const int pageSize = 8;
            if (pg < 1)
                pg = 1;
            int recsCount = doitac.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = doitac.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.doitac = data;
            return View(trang_Thais);
        }


        private void temp6(int id, object selecttemp5 = null)
        {
            ViewBag.Compani = new SelectList(db.TrangThais.Where(s => s.Ma_Trang_Thai == id).ToList(), "Ma_Trang_Thai", "Ten_Trang_Thai", selecttemp5);

        }

        [Route("/Partner/Home/list2/{maloai}", Name = "ShopList2")]
        public IActionResult ShopList2(int maloai, int pg = 1)
        {

            //ViewBag.id = maloai;
            List<NongSan> nongSans = db.NongSans.ToList();
            List<GianHang> gianHangs = db.GianHangs.ToList();
            //List<DonDat> donDats = db.DonDats.Where(x => x.MaTrangThai == maloai && x.MaDonDat == 1).ToList();
            List<DonDat> donDats = db.DonDats.Where(x => x.MaTrangThai == maloai).ToList();
            List<NguoiDung> nguoiDungs = db.NguoiDungs.ToList();
            List<ChiTietNsDd> chiTietNsDds = db.ChiTietNsDds.ToList();
            List<HinhThucThanhToan> hinhThucThanhToans = db.HinhThucThanhToans.ToList();
            List<Trang_Thai> trang_Thais = db.TrangThais.ToList();
            string id = HttpContext.Session.GetString("taikhoan");
            int ma = Convert.ToInt32(id);
            //temp6(maloai);
            HttpContext.Session.SetInt32("matrangthaidd", maloai);
            temp7();
            var doitac = (from d in donDats
                          join c in chiTietNsDds on d.MaDonDat equals c.MaDonDat into table1
                          from c in table1.DefaultIfEmpty()
                          join n in nongSans on c.MaNongSan equals n.MaNongSan into table2
                          from n in table2.DefaultIfEmpty()
                          join g in gianHangs on n.MaGianHang equals g.MaGianHang into table3
                          from g in table3.DefaultIfEmpty()
                          join nd in nguoiDungs on g.MaNguoiDung equals nd.MaNguoiDung into table4
                          from nd in table4.DefaultIfEmpty()
                          join hh in hinhThucThanhToans on d.MaHttt equals hh.MaHtth into table5
                          from hh in table5.DefaultIfEmpty()
                          join tt in trang_Thais on d.MaTrangThai equals tt.Ma_Trang_Thai
                          where nd.MaTaiKhoan == ma && n.TrangThai == 1
                          select new PartnerContent
                          {
                              nongsandetail = n,
                              gianhangdetail = g,
                              nguoidungdetail = nd,
                              hinhthucdetail = hh,
                              trangthaidetail = tt,
                              dondatdetail = d,
                              chitietdetail = c,
                              mattdd = maloai

                          });
            const int pageSize = 8;
            if (pg < 1)
                pg = 1;
            int recsCount = doitac.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = doitac.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            ViewBag.doitac = data;
            return View(doitac);
        }


    }
}
