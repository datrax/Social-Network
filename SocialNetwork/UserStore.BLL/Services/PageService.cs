﻿using System;
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
    }
}
