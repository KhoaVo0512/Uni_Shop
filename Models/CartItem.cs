using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni_Shop.ModelDBs;

namespace Uni_Shop.Models
{
    public class CartItem
    {
        public int MaNongSan { get; set; }
        public string TenNongSan { get; set; }
        public int SL { get; set; }
        public string Gia { get; set; }
    }
}
