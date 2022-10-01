using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Uni_Shop.ModelDBs
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            NguoiDungs = new HashSet<NguoiDung>();
            NhanViens = new HashSet<NhanVien>();
        }
       
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(255)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Vui lòng nhập mật khẩu ít nhất một ký tự in hoa, ký tự thường, ký tự đặc biệt và chữ số")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress]
        public string Email { get; set; }
        public int? MaQuyen { get; set; }

        public virtual Quyen MaQuyenNavigation { get; set; }
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
