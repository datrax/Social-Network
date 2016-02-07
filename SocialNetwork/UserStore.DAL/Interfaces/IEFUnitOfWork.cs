using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface IEFUnitOfWork
    {
        IRepository<ClientProfile> Users { get; }
        IRepository<Post> Posts { get; }
        IRepository<Photo> Avatars { get; } 
  //      IRepository<LikesUserPost> PostLikes { get; }
        void Save();
    }
}
