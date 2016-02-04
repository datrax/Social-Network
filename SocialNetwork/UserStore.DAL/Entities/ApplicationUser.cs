using Microsoft.AspNet.Identity.EntityFramework;

namespace UserStore.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
