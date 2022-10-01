using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uni_Shop.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu cũ"), DataType(DataType.Password), Display(Name = "Mật khẩu cũ"),]
        public string CurrenPassword { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới"), DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới"), DataType(DataType.Password), Display(Name = "Nhập lại mật khẩu mới")]
        
        //[Compare("NewPassword", ErrorMessage = "Mật khẩu mới không chính xác")]
        public string ConfimNewPassword { get; set; }
    }
}
