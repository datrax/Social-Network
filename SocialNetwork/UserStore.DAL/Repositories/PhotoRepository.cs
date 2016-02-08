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

    internal class PhotoRepository : IRepository<Photo>
    {
        private ApplicationContext db;

        public PhotoRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Photo> GetAll()
        {
            return db.Avatars;
        }

        public Photo Get(int id)
        {
            return db.Avatars.Find(id);
        }

        public void Create(Photo photo)
        {
            db.Avatars.Add(photo);
        }

        public void Update(Photo photo)
        {
            //db.Entry(photo).State = EntityState.Modified;
            var t=db.Avatars.Where(a => a.UserId == photo.UserId).First();
            t.Avatar = photo.Avatar;
        }

        public IEnumerable<Photo> Find(Func<Photo, Boolean> predicate)
        {
            return db.Avatars.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Photo photo = db.Avatars.Find(id);
            if (photo != null)
                db.Avatars.Remove(photo);
        }

    }
}
