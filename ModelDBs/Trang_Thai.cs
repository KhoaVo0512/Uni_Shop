using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uni_Shop.ModelDBs
{
    public partial class Trang_Thai
    {
        public Trang_Thai()
        {
            DonDats = new HashSet<DonDat>();
        }
       
        public int Ma_Trang_Thai { get; set; }
        public string Ten_Trang_Thai { get; set; }

        public virtual ICollection<DonDat> DonDats { get; set; }
    }
}
