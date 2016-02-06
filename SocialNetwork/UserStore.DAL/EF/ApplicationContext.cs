using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UserStore.DAL.Entities;

namespace UserStore.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<LikesUserPost> LikesUserPost { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.LikesUserPost)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClientProfile>()
                .Property(e => e.Name)
                .IsUnicode(false);


            modelBuilder.Entity<ClientProfile>()
                .HasMany(e => e.LikesUserPost)
                .WithRequired(e => e.ClientProfile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClientProfile>()
                .HasMany(e => e.PostedPost)
                .WithRequired(e => e.UserFrom)
                .HasForeignKey(e => e.UserFromId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClientProfile>()
                .HasMany(e => e.OnWallPosts)
                .WithRequired(e => e.UserTo)
                .HasForeignKey(e => e.UserToId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}
