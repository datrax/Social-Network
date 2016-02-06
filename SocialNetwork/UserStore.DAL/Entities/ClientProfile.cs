using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserStore.DAL.Entities
{
    public class ClientProfile
    {
        public ClientProfile()
        {
            LikesUserPost = new HashSet<LikesUserPost>();
            PostedPost = new HashSet<Post>();
            OnWallPosts = new HashSet<Post>();
        }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikesUserPost> LikesUserPost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> PostedPost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> OnWallPosts { get; set; }
        //  public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
