using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uni_Shop.ModelDBs
{
    public class DonXinDT
    {
        public int MaDoiTac { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public int MaNguoiDung { get; set; }

        public virtual NguoiDung MaNguoiDungNavigation { get; set; }
    }
}
