using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            BaiViets = new HashSet<BaiViet>();
        }

        public int MaNhanVien { get; set; }
        public int? MaTaiKhoan { get; set; }
        [Required]
        public string TenNhanVien { get; set; }
        [Required]
        public string DiaChi { get; set; }
        //[RegularExpression(@"0\d{9,10}", ErrorMessage = "Vui nhập đúng số điện thoại bất đầu từ 0 và chiều dài 10")]
        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone]
        public string Sdt { get; set; }
        public string Avatar { get; set; }

        public virtual TaiKhoan MaTaiKhoanNavigation { get; set; }
        public virtual ICollection<BaiViet> BaiViets { get; set; }
    }
}
