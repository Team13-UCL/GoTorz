using GoTorz.Data;
using GoTorz.Model;
using Microsoft.EntityFrameworkCore;

namespace GoTorz.Services
{
   public class UserService
    {
        private readonly AuthContext context;

        public UserService(AuthContext context)
        {
            this.context = context;
        }

        public bool SaveUser(User user) //Register users
        {
            bool isExist = context.Users.Any(x => x.Email == user.Email); // .Any checks if the email is already in use
            if (!isExist)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true; //Returns true if the User is being registered succesfully
            }
            return false; //Returns false if it already exists or there is an error
        }

        public User? Verify(string email, string password)
        {
            return context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower()
            && x.Password == password);
        }
        public User? GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await context.SaveChangesAsync();
            return true;
        }
    }
}