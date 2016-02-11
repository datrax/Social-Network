using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UserStore.BLL.DTO;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.Repositories;

namespace UserStore.BLL.Services
{
    public class PageService : IPageService
    {
        private IEFUnitOfWork Database { get; set; }

        public PageService()
        {
            Database = new EFUnitOfWork();
        }

        public UserDTO GetUserByID(string id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientProfile, UserDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<UserDTO>(Database.Users.Find(a => a.Id == id).First());
        }

        public UserDTO GetUserByLogin(string login)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientProfile, UserDTO>());
            var mapper = config.CreateMapper();
            var t = Database.Users.Find(a => a.Login == login).ToList();
            if (t.Count == 0)
                return null;
            return mapper.Map<UserDTO>(Database.Users.Find(a => a.Login == login).First());
        }

        public AvatarDTO GetAvatar(string login)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Photo, AvatarDTO>());
            var mapper = config.CreateMapper();
            var t = Database.Avatars.Find(a => a.UserId == login).ToList();
            if (t.Count == 0)
                return null;
            return mapper.Map<AvatarDTO>(Database.Avatars.Find(a => a.UserId == login).First());

        }

        public bool SetAvatar(AvatarDTO avatar)
        {
            try
            {
                var t = Database.Avatars.Find(a => a.UserId == avatar.UserId).ToList();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<AvatarDTO, Photo>());
                var mapper = config.CreateMapper();
                if (t.Count == 0)
                {
                    Database.Avatars.Create(mapper.Map<Photo>(avatar));
                }
                else
                {
                    Database.Avatars.Update(mapper.Map<Photo>(avatar));
                }
                Database.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeUserInfo(UserDTO user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, ClientProfile>());
            var mapper = config.CreateMapper();
            Database.Users.Update(mapper.Map<ClientProfile>(user));
            Database.Save();
            return true;
        }

        public IEnumerable<UserDTO> FindUsers(string input)
        {
            var t = input.IndexOf(" ");
            string name = "", surname = "";
            if (t > 0)
            {
                name = input.Substring(0, t);
                surname = input.Substring(t + 1);
            }
            else
            {
                name = input;
            }
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname))
                return new List<UserDTO>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientProfile, UserDTO>());
            var mapper = config.CreateMapper();
            if (name == "")
            {
                return
                           mapper.Map<List<UserDTO>>(
                               Database.Users.Find(a => a.Surname.ToLower().Contains(surname.ToLower())));


            }
            else
            {
                if (surname == "")
                {
                    return mapper.Map<List<UserDTO>>(Database.Users.Find(a => a.Name.ToLower().Contains(name.ToLower())));
                }
                return
                    mapper.Map<List<UserDTO>>(
                        Database.Users.Find(
                            a =>
                                a.Name.ToLower().Contains(name.ToLower()) &&
                                a.Surname.ToLower().Contains(surname.ToLower())).ToList());
            }
        }

        public IEnumerable<PostDTO> GetPosts(string authorizeId, string urlId)
        {
            var user = Database.Users.Find(a => a.Id == urlId).First();
            var posts = user.OnWallPosts.ToList();
            List<PostDTO> wallPosts = new List<PostDTO>();
            foreach (var post in posts)
            {
                wallPosts.Add(new PostDTO()
                {
                    Id = post.Id,
                    DateTime = post.DateTime,
                    Name = post.UserFrom.Name,
                    Surname = post.UserFrom.Surname,
                    Text = post.Text,
                    CanDelete = authorizeId == post.UserFromId || authorizeId == post.UserToId,
                    UserId = post.UserFromId,
                    LikesCount = post.LikesUserPost.Count(),
                    Login = post.UserFrom.Login
                });
            }
            return wallPosts.OrderByDescending(a => a.DateTime);
        }
        public bool AddPost(string authorizeId, string urlId, string text)
        {
            Post post = new Post()
            {
                DateTime = DateTime.Now,
                Text = text,
                UserFromId = authorizeId,
                UserToId = urlId
            };
            Database.Posts.Create(post);
            Database.Save();
            return true;
        }
        public bool DeletePost(int postId)
        {
            Database.Posts.Delete(postId);
            Database.Save();
            return true;
        }
        public bool LikePost(string authorizeId, int postId)
        {
            var user = Database.Users.Find(a => a.Id == authorizeId).First();
            var post = Database.Posts.Get(postId);
            var t=post.LikesUserPost.Where(a => a.ClientProfileId == authorizeId).ToList();
            
            if (t.Count == 0)
            {
                post.LikesUserPost.Add(new LikesUserPost()
                {
                    PostId = postId,
                    ClientProfileId = authorizeId
                });
                
            }
            else
            {
                Database.Likes.Delete(t.First().Id);
            }
            Database.Save();
            return true;
        }
    }
}