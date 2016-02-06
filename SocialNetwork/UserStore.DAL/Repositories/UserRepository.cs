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
    class UserRepository : IRepository<ClientProfile>
    {
        private ApplicationContext db;

        public UserRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            return db.ClientProfiles;
        }

        public ClientProfile Get(int id)
        {
            return db.ClientProfiles.Find(id);
        }

        public void Create(ClientProfile profile)
        {
            db.ClientProfiles.Add(profile);
        }

        public void Update(ClientProfile profile)
        {
            db.Entry(profile).State = EntityState.Modified;
        }

        public IEnumerable<ClientProfile> Find(Func<ClientProfile, Boolean> predicate)
        {
            return db.ClientProfiles.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            ClientProfile profile = db.ClientProfiles.Find(id);
            if (profile != null)
                db.ClientProfiles.Remove(profile);
        }

    }
}
