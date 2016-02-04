using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserStore.DAL.Entities
{
    public class ClientProfile
    {

        public string Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

      //  public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
