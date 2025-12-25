using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ValeraProject.Data;
using ValeraProject.Models;

namespace ValeraProject.Services
{
    public interface IValeraService
    {
        Task<List<Valera>> GetAllValerasAsync(ClaimsPrincipal user);
        Task<List<Valera>> GetMyValerasAsync(ClaimsPrincipal user);
        Task<Valera?> GetValeraByIdAsync(int id, ClaimsPrincipal user);
        Task<Valera> CreateValeraAsync(Valera valera, ClaimsPrincipal user);
        Task<bool> DeleteValeraAsync(int id, ClaimsPrincipal user);
        Task<Valera?> PerformActionAsync(int id, string action, ClaimsPrincipal user);
    }

    public class ValeraService : IValeraService
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public ValeraService(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

public async Task<List<Valera>> GetAllValerasAsync(ClaimsPrincipal user)
{
    // Только админ может видеть всех Валер
    if (_authService.GetUserRoleFromToken(user) != "Admin")
        throw new UnauthorizedAccessException("Only admin can view all valeras");

    return await _context.Valeras.Include(v => v.User).ToListAsync();
}
        public async Task<List<Valera>> GetMyValerasAsync(ClaimsPrincipal user)
        {
            var userId = _authService.GetUserIdFromToken(user);
            return await _context.Valeras
                .Where(v => v.UserId == userId)
                .ToListAsync();
        }

        public async Task<Valera?> GetValeraByIdAsync(int id, ClaimsPrincipal user)
        {
            var valera = await _context.Valeras
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (valera == null)
                return null;

            // Проверяем права доступа
            var userId = _authService.GetUserIdFromToken(user);
            var userRole = _authService.GetUserRoleFromToken(user);
            
            if (userRole != "Admin" && valera.UserId != userId)
                return null;

            return valera;
        }

        public async Task<Valera> CreateValeraAsync(Valera valera, ClaimsPrincipal user)
        {
            var userId = _authService.GetUserIdFromToken(user);
            valera.UserId = userId;

            // Убеждаемся, что новый Валера создается с корректными значениями
            valera.Health = Math.Max(0, Math.Min(100, valera.Health));
            valera.Mana = Math.Max(0, Math.Min(100, valera.Mana));
            valera.Cheerfulness = Math.Max(-10, Math.Min(10, valera.Cheerfulness));
            valera.Fatigue = Math.Max(0, Math.Min(100, valera.Fatigue));
            valera.Money = Math.Max(0, valera.Money);
            
            _context.Valeras.Add(valera);
            await _context.SaveChangesAsync();
            return valera;
        }

        public async Task<bool> DeleteValeraAsync(int id, ClaimsPrincipal user)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
                return false;

            // Проверяем права доступа
            var userId = _authService.GetUserIdFromToken(user);
            var userRole = _authService.GetUserRoleFromToken(user);
            
            if (userRole != "Admin" && valera.UserId != userId)
                return false;

            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Valera?> PerformActionAsync(int id, string action, ClaimsPrincipal user)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
                return null;

            // Проверяем права доступа
            var userId = _authService.GetUserIdFromToken(user);
            var userRole = _authService.GetUserRoleFromToken(user);
            
            if (userRole != "Admin" && valera.UserId != userId)
                return null;

            if (valera.Health <= 0)
                return null;

            bool actionResult = false;

            switch (action.ToLower())
            {
                case "work":
                    actionResult = valera.GoToWork();
                    break;
                case "nature":
                    valera.ContemplateNature();
                    actionResult = true;
                    break;
                case "wine":
                    actionResult = valera.DrinkWineAndWatchTV();
                    break;
                case "bar":
                    actionResult = valera.GoToBar();
                    break;
                case "marginals":
                    actionResult = valera.DrinkWithMarginals();
                    break;
                case "sing":
                    valera.SingInMetro();
                    actionResult = true;
                    break;
                case "sleep":
                    valera.Sleep();
                    actionResult = true;
                    break;
                default:
                    actionResult = false;
                    break;
            }

            if (!actionResult)
                return null;

            await _context.SaveChangesAsync();
            return valera;
        }
    }
}