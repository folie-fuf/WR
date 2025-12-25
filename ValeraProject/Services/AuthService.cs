using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ValeraProject.Data;
using ValeraProject.DTOs;
using ValeraProject.Models;

namespace ValeraProject.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> Register(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
        int GetUserIdFromToken(ClaimsPrincipal user);
        string GetUserRoleFromToken(ClaimsPrincipal user);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> Register(RegisterDto registerDto)
        {
            // Проверяем, существует ли пользователь с таким email
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return null;

            // Проверяем, существует ли пользователь с таким username
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                return null;

            // Создаем пользователя
            var user = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Генерируем токен
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            // Находим пользователя по email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                return null;

            // Генерируем токен
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id
            };
        }

        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        public string GetUserRoleFromToken(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? "User";
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}