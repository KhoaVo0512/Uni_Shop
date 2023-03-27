using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uni_Shop.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu cũ"), DataType(DataType.Password), Display(Name = "Mật khẩu cũ")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Vui lòng nhập mật khẩu ít nhất một ký tự in hoa, ký tự thường, ký tự đặc biệt và chữ số")]
        public string CurrenPassword { get; set; }
        [StringLength(20)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Vui lòng nhập mật khẩu ít nhất một ký tự in hoa, ký tự thường, ký tự đặc biệt và chữ số")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới"), DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới"), DataType(DataType.Password), Display(Name = "Nhập lại mật khẩu mới")]
        
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không chính xác")]
        public string ConfimNewPassword { get; set; }
    }
}
