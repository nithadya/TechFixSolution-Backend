using Microsoft.EntityFrameworkCore;
using TechFixSolution.AuthServices.Data;
using TechFixSolution.AuthServices.Models;

namespace TechFixSolution.AuthServices.Services
{
    public class AuthService
    {
        private readonly UserContext _context;

        public AuthService(UserContext context)
        {
            _context = context;
        }

        // Authenticate user by checking username and password
        public User Authenticate(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) // Verifying the hashed password
            {
                return null;
            }

            return user;
        }

        // Register a new user (Admin or Supplier) with hashed password
        public User Register(RegisterRequest model)
        {
            // Check if the username already exists
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                return null; // User already exists
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password), // Hashing the password
                Role = model.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        // Read a user by ID
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Update user details
        public User UpdateUser(int id, UpdateUserRequest model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.Username = model.Username ?? user.Username;
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Hashing the new password
            user.Role = model.Role ?? user.Role;

            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        // Delete a user
        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
