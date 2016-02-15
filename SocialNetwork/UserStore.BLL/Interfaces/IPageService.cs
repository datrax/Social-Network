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
        string ChangeUserInfo(UserDTO user);
        IEnumerable<UserDTO> FindUsers(string input);
        IEnumerable<PostDTO> GetPosts(string authorizeId, string urlId);
        bool DeletePost(int postId,string postWallOwnerId);
        bool AddPost(string authorizeId, string urlId, string text, byte[] image);
        bool LikePost(string authorizeId, int postId);
        IEnumerable<UserDTO> GetLikeUserList(int postId);
        string GetPostWallOwnerById(int postId);
    }
}
