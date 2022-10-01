using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            DonDats = new HashSet<DonDat>();
            GianHangs = new HashSet<GianHang>();
        }

        public int MaNguoiDung { get; set; }
        public int? MaTaiKhoan { get; set; }
        public string TenNguoiDung { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Avatar { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; }
        public virtual ICollection<DonDat> DonDats { get; set; }
        public virtual ICollection<GianHang> GianHangs { get; set; }
        public virtual ICollection<DonXinDT> DonXinDTs { get; set; }
    }
}
