using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class NongSan
    {
        public NongSan()
        {
            AnhNs = new HashSet<AnhN>();
            ChiTietNsDds = new HashSet<ChiTietNsDd>();
            GioHangs = new HashSet<GioHang>();
        }

        public int MaNongSan { get; set; }
        public int MaLoaiNongSan { get; set; }
        public int MaGianHang { get; set; }
        public int MaDonViTinh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên nông sản")]
        public string TenNongSan { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trọng lượng")]
        [RegularExpression(pattern: "[^0-9]", ErrorMessage = "Vui lòng nhập ký tự số")]
        public string TrongLuong { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [RegularExpression(pattern: "[^0-9]", ErrorMessage = "Vui lòng nhập ký tự số")]
        public string SoLuong { get; set; }
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá tiền")]
        [RegularExpression(pattern: "[^0-9]", ErrorMessage = "Vui lòng nhập ký tự số")]
        public string Gia { get; set; }
        public int? TrangThai { get; set; }

        public virtual DonViTinh MaDonViTinhNavigation { get; set; }
        public virtual GianHang MaGianHangNavigation { get; set; }
        public virtual LoaiNongSan MaLoaiNongSanNavigation { get; set; }
        public virtual ICollection<AnhN> AnhNs { get; set; }
        public virtual ICollection<ChiTietNsDd> ChiTietNsDds { get; set; }
        public virtual ICollection<GioHang> GioHangs { get; set; }
    }
}
