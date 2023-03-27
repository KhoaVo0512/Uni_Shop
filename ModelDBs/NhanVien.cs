using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            BaiViets = new HashSet<BaiViet>();
        }

        public int MaNhanVien { get; set; }
        public int? MaTaiKhoan { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Avatar { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; }
        public virtual ICollection<BaiViet> BaiViets { get; set; }
    }
}
