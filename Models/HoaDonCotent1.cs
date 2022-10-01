using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;
namespace Uni_Shop.Models
{
    public class HoaDonCotent1
    {
        public HoaDon HoaDondetail { get; set; }
        public DonDat DonDatdeltai { get; set; }
        public NguoiDung NguoiDungdeltai { get; set; }
        public Trang_Thai trang_thai { get; set; }
    }
}
