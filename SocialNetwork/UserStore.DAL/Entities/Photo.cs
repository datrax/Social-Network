using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.DAL.Entities
{
    public class Photo
    {
        [Key]
        public string UserId { get; set; }
        public byte[] Avatar { get; set; }

    }
}
