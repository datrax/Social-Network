﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    public class EFUnitOfWork:IEFUnitOfWork
    {
        private ApplicationContext db;
        private UserRepository userRepository;
        private PostRepository postRepository;
        private PhotoRepository photoRepository;
        private LikeUserRepository likeUserRepository;

        public EFUnitOfWork()
        {
            db = new ApplicationContext();
        }
        public IRepository<Post> Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }

        public IRepository<ClientProfile> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public IRepository<Photo> Avatars
        {
            get
            {
                if (photoRepository == null)
                    photoRepository = new PhotoRepository(db);
                return photoRepository;
            }
        }
        public IRepository<LikesUserPost> Likes
        {
            get
            {
                if (likeUserRepository == null)
                    likeUserRepository = new LikeUserRepository(db);
                return likeUserRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
