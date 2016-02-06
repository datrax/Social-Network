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
            Database =new EFUnitOfWork("data source = WIN - H9FLMNV32VF\\SQLEXPRESS; initial catalog = SimpleSocialNetwork4; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework");
        }
        public UserDTO GetUserByID(string login)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ClientProfile, UserDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<UserDTO>(Database.Users.Find(a => a.Id == login).First());
        }
    }
}
