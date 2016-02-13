using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    class LikeUserRepository : IRepository<LikesUserPost>
    {
        private ApplicationContext db;

        public LikeUserRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<LikesUserPost> GetAll()
        {
            return db.LikesUserPost;
        }

        public LikesUserPost Get(int id)
        {
            return db.LikesUserPost.Find(id);
        }

        public LikesUserPost Create(LikesUserPost profile)
        {
            db.LikesUserPost.Add(profile);
            db.SaveChanges();
            return profile;
        }

        public void Update(LikesUserPost profile)
        {
            db.Entry(profile).State = EntityState.Modified;
        }

        public IEnumerable<LikesUserPost> Find(Func<LikesUserPost, Boolean> predicate)
        {
            return db.LikesUserPost.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            LikesUserPost post = db.LikesUserPost.Find(id);
            if (post != null)
                db.LikesUserPost.Remove(post);
        }
    }
}


