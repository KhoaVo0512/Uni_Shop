using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class LoaiNongSan
    {
        public LoaiNongSan()
        {
            NongSans = new HashSet<NongSan>();
        }

        public int MaLoaiNongSan { get; set; }
        [Required]
        public string TenLoaiNongSan { get; set; }

        public virtual ICollection<NongSan> NongSans { get; set; }
    }
}
