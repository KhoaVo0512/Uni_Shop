using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
namespace Uni_Shop.Models
{
    public class NongSanChiTiet
    {
        public NongSan nongsandetail { get; set; }
        public AnhN anhnsdetail { get; set; }

        public DonViTinh donvitinhdetail { get; set; }
        public LoaiNongSan loainongsandeltail { get; set; }
        public GianHang gianhangdetail { get; set; }
  
    }
}
