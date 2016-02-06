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
    class PostRepository:IRepository<Post>
    {
        private ApplicationContext db;

        public PostRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Post> GetAll()
        {
            return db.Post;
        }

        public Post Get(int id)
        {
            return db.Post.Find(id);
        }

        public void Create(Post profile)
        {
            db.Post.Add(profile);
        }

        public void Update(Post profile)
        {
            db.Entry(profile).State = EntityState.Modified;
        }

        public IEnumerable<Post> Find(Func<Post, Boolean> predicate)
        {
            return db.Post.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Post post = db.Post.Find(id);
            if (post != null)
                db.Post.Remove(post);
        }
    }
}
