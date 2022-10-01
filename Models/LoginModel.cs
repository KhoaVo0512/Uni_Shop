using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Models
{
    public class LoginModel
    {
        public int MaTaiKhoan { set; get; }
        public string TenDangNhap { set; get; }
        public string MatKhau { set; get; }
        public bool Remember { set; get; }

     }
}
