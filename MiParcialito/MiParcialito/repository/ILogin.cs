using MiParcialito.Models;

namespace MiParcialito.repository
{
    public interface ILogin
    {
         Task<IEnumerable<AspNetUser>> getuser();
         Task<AspNetUser> AuthenticateUser(string username,string passcode);
    }
}
