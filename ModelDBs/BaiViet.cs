using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class BaiViet
    {
        public int MaTinTuc { get; set; }
        public int? MaNhanVien { get; set; }
        public string LinkAnh { get; set; }
        public string NoiDung { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; }
    }
}
