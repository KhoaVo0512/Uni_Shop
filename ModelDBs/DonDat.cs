using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class DonDat
    {
        public DonDat()
        {
            ChiTietNsDds = new HashSet<ChiTietNsDd>();
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaDonDat { get; set; }
        public string NgayDatHang { get; set; }
        public int? MaTrangThai { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaHttt { get; set; }
        public string MaGd { get; set; }

        public virtual HinhThucThanhToan MaHtttNavigation { get; set; }
        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
        public virtual Trang_Thai MaTrangThaiNavigation { get; set; }
        public virtual ICollection<ChiTietNsDd> ChiTietNsDds { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
