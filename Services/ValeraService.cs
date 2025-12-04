using ValeraProject.Data;
using ValeraProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ValeraProject.Services
{
    public interface IValeraService
    {
        Task<List<Valera>> GetAllValerasAsync();
        Task<Valera?> GetValeraByIdAsync(int id);
        Task<Valera> CreateValeraAsync(Valera valera);
        Task<bool> DeleteValeraAsync(int id);
        Task<Valera?> PerformActionAsync(int id, string action);
    }

    public class ValeraService : IValeraService
    {
        private readonly AppDbContext _context;

        public ValeraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Valera>> GetAllValerasAsync()
        {
            return await _context.Valeras.ToListAsync();
        }

        public async Task<Valera?> GetValeraByIdAsync(int id)
        {
            return await _context.Valeras.FindAsync(id);
        }

        public async Task<Valera> CreateValeraAsync(Valera valera)
        {
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

        public async Task<bool> DeleteValeraAsync(int id)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
                return false;

            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Valera?> PerformActionAsync(int id, string action)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
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