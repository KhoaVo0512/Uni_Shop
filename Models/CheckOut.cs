using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Models
{
    public class CheckOut
    {
        public IEnumerable<CartContent> cartcontentdetail { get; set; }
        public NguoiDung nguoidungdetail { get; set; }
        public double ThanhTien { get; set; }
    }
}
