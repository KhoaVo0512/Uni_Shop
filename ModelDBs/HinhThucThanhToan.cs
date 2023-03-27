using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class HinhThucThanhToan
    {
        public HinhThucThanhToan()
        {
            DonDats = new HashSet<DonDat>();
        }

        public int MaHtth { get; set; }
        public string TenHTTT { get; set; }

        public virtual ICollection<DonDat> DonDats { get; set; }
    }
}
