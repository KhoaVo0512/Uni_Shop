using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uni_Shop.ModelDBs
{
    public class GioHang
    {
        public int MaNongSan { get; set; }
        public int MaTaiKhoan { get; set; }
        public int? SL { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; }
        public virtual NongSan MaNongSanNavigation { get; set; }
    }
}
