using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class GianHang
    {
        public GianHang()
        {
            NongSans = new HashSet<NongSan>();
        }

        public int MaGianHang { get; set; }
        public string TenGianHang { get; set; }
        public int MaNguoiDung { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
        public virtual ICollection<NongSan> NongSans { get; set; }
    }
}
