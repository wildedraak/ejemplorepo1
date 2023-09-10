using Microsoft.EntityFrameworkCore;
using MiParcialito.Models;

namespace MiParcialito.repository
{
    public class AuthenticateLogin : ILogin
    {
        private readonly rb100519Context _context;
        public AuthenticateLogin(rb100519Context context)
        {
            _context = context;
        }
        public async Task<AspNetUser> AuthenticateUser(string username, string passcode)
        {
            var succeeded = await _context.AspNetUsers.FirstOrDefaultAsync(authUser => authUser.Email == username && authUser.PasswordHash == passcode);
            return succeeded;
        }

        public async Task<IEnumerable<AspNetUser>> getuser()
        {
            return await _context.AspNetUsers.ToListAsync();
        }
    }
}
