using System;
using System.Collections.Generic;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class AnhN
    {
        public int MaAnh { get; set; }
        public string LinkAnh { get; set; }
        public int MaNongSan { get; set; }
        public string MoTa { get; set; }

        public virtual NongSan MaNongSanNavigation { get; set; }
    }
}
