using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Vui lòng nhập tên loại nông sản")]
        public string TenDonViTinh { get; set; }

        public virtual ICollection<NongSan> NongSans { get; set; }
    }
}
