namespace UserStore.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LikesUserPost")]
    public partial class LikesUserPost
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string ClientProfileId { get; set; }


        public virtual Post Post { get; set; }

        public virtual ClientProfile ClientProfile { get; set; }
    }
}
