using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class NongSan
    {
        public NongSan()
        {
            AnhNs = new HashSet<AnhN>();
        }

        public int MaNongSan { get; set; }
        public int MaLoaiNongSan { get; set; }
        public int MaGianHang { get; set; }
        public int MaDonViTinh { get; set; }
        public string TenNongSan { get; set; }
        public string TrongLuong { get; set; }
        public string SoLuong { get; set; }
        public string MoTa { get; set; }
        public string Gia { get; set; }
        public int TrangThai{ get; set; }

        public virtual DonViTinh MaDonViTinhNavigation { get; set; }
        public virtual GianHang MaGianHangNavigation { get; set; }
        public virtual LoaiNongSan MaLoaiNongSanNavigation { get; set; }
        public virtual ICollection<AnhN> AnhNs { get; set; }
    }
}
