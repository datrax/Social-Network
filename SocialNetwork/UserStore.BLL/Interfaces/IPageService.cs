using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;

namespace UserStore.BLL.Interfaces
{
    public interface IPageService
    {
        UserDTO GetUserByID(string id);
        UserDTO GetUserByLogin(string login);
        AvatarDTO GetAvatar(string login);
        bool SetAvatar(AvatarDTO avatar);
        bool ChangeUserInfo(UserDTO user);
    }
}
