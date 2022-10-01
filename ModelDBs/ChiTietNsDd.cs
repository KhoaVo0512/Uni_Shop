using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class ChiTietNsDd
    {
        public int MaNongSan { get; set; }
        public int MaDonDat { get; set; }
        public int? SoLuongDat { get; set; }

        public virtual DonDat MaDonDatNavigation { get; set; }
        public virtual NongSan MaNongSanNavigation { get; set; }
    }
}
