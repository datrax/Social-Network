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
    public class PageService:IPageService
    {
        IEFUnitOfWork Database { get; set; }
        public PageService()
        {     
            Database =new EFUnitOfWork();
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
            if (t.Count==0)
                return null;
            return mapper.Map<UserDTO>(Database.Users.Find(a => a.Login == login).First());
        }

        public AvatarDTO GetAvatar(string login)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Photo, AvatarDTO>());
            var mapper = config.CreateMapper();
            var t = Database.Avatars.Find(a => a.Login == login).ToList();
            if (t.Count == 0)
                return null;
            return mapper.Map<AvatarDTO>(Database.Avatars.Find(a => a.Login == login).First());

        }

        public bool SetAvatar(AvatarDTO avatar)
        {
            try
            {
                var t = Database.Avatars.Find(a => a.Login == avatar.Login).ToList();
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
    }
}
