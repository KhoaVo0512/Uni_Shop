using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class GioHang
    {
        public int MaTaiKhoan { get; set; }
        public int MaNongSan { get; set; }
        public int? Sl { get; set; }

        public virtual NongSan MaNongSanNavigation { get; set; }
        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; }
    }
}
