using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class DonXinDT
    {
        public int MaDoiTac { get; set; }
        public int? MaNguoiDung { get; set; }
        public DateTime? NgayDuyet { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
