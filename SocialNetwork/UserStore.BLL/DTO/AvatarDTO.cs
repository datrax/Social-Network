using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.DTO
{
    public class AvatarDTO
    {
        public string Login { get; set; }
        public byte[] Avatar { get; set; }
    }
}
