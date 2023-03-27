using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
namespace Uni_Shop.Models
{
    public class PartnerContent
    {
        public GianHang gianhangdetail { get; set; }
        public DonDat dondatdetail { get; set; }
        public NongSan nongsandetail { get; set; }
        public NguoiDung nguoidungdetail { get; set; }
        public int mattdd { get; set; }
        public ChiTietNsDd chitietdetail { get; set; }
        public HinhThucThanhToan hinhthucdetail { get; set; }
        public Trang_Thai trangthaidetail { get; set; }
    }
}
