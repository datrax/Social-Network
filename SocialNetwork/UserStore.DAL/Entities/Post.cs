namespace UserStore.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
           LikesUserPost = new HashSet<LikesUserPost>();
        }

        public int Id { get; set; }

        public string Text { get; set; }
        [Required]
        public bool HasPhoto { get; set; }

        public string UserFromId { get; set; }

        public string UserToId { get; set; }

        public DateTime DateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikesUserPost> LikesUserPost { get; set; }

        public virtual ClientProfile UserFrom { get; set; }

        public virtual ClientProfile UserTo { get; set; }
    }
}
