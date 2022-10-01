using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uni_Shop.Models
{
    public class ghdd
    {
        public GianHang gianhangdetail { get; set; }
        public DonDat dondatdetail { get; set; }
        public NguoiDung nguoidungdetail { get; set; }
        public TaiKhoan taikhoandetail { get; set; }
        public Quyen quyendetail { get; set; }
        public NongSan nongsandetail { get; set; }
        public ChiTietNsDd chitietdetail { get; set; }
        public Trang_Thai trangthaidetail { get; set; }
        public string TT { get; set; }
    }
}
