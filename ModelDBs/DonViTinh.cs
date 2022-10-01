using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class DonViTinh
    {
        public DonViTinh()
        {
            NongSans = new HashSet<NongSan>();
        }

        public int MaDonViTinh { get; set; }
        public string TenDonViTinh { get; set; }

        public virtual ICollection<NongSan> NongSans { get; set; }
    }
}
