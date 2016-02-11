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
        IEnumerable<UserDTO> FindUsers(string input);
        IEnumerable<PostDTO> GetPosts(string authorizeId, string urlId);
        bool DeletePost(int postId);
        bool AddPost(string authorizeId, string urlId, string text);
        bool LikePost(string authorizeId, int postId);
    }
}
