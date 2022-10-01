using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class HoaDon
    {
        public int MaHoaDon { get; set; }
        public string NgayGiaoHang { get; set; }
        public int MaDonDat { get; set; }

        public virtual DonDat MaDonDatNavigation { get; set; }
    }
}
