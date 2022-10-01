using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Shop.Models
{
    public class NongSanContentt
    {
        public NongSan nongsandetail { get; set; }
        public GianHang gianhangdetail { get; set; }
        public NguoiDung nguoidungdetail { get; set; }
        public TaiKhoan taikhoandetail { get; set; }
        public DonViTinh donvitinhdetail { get; set; }
        public LoaiNongSan loainongsandeltail { get; set; }
        public ChiTietNsDd chitietdetail { get; set; }
        public DonDat dondatdetail { get; set; }
        public Quyen quyendetail { get; set; }
       public AnhN anhdetail { get; set; }
      

    }
}
