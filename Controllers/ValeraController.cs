using Microsoft.AspNetCore.Mvc;
using ValeraProject.Services;
using ValeraProject.Models;

namespace ValeraProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        // Существующие методы
        [HttpGet]
        public async Task<ActionResult<List<Valera>>> GetAllValeras()
        {
            var valeras = await _valeraService.GetAllValerasAsync();
            return Ok(valeras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Valera>> GetValera(int id)
        {
            var valera = await _valeraService.GetValeraByIdAsync(id);
            if (valera == null)
                return NotFound();
            return Ok(valera);
        }

        [HttpPost]
        public async Task<ActionResult<Valera>> CreateValera(Valera valera)
        {
            var createdValera = await _valeraService.CreateValeraAsync(valera);
            return CreatedAtAction(nameof(GetValera), new { id = createdValera.Id }, createdValera);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValera(int id)
        {
            var result = await _valeraService.DeleteValeraAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // НОВЫЕ МЕТОДЫ ДЛЯ ДЕЙСТВИЙ
        [HttpPost("{id}/work")]
        public async Task<ActionResult<Valera>> Work(int id)
        {
            return await PerformAction(id, "work");
        }

        [HttpPost("{id}/nature")]
        public async Task<ActionResult<Valera>> ContemplateNature(int id)
        {
            return await PerformAction(id, "nature");
        }

        [HttpPost("{id}/wine")]
        public async Task<ActionResult<Valera>> DrinkWine(int id)
        {
            return await PerformAction(id, "wine");
        }

        [HttpPost("{id}/bar")]
        public async Task<ActionResult<Valera>> GoToBar(int id)
        {
            return await PerformAction(id, "bar");
        }

        [HttpPost("{id}/marginals")]
        public async Task<ActionResult<Valera>> DrinkWithMarginals(int id)
        {
            return await PerformAction(id, "marginals");
        }

        [HttpPost("{id}/sing")]
        public async Task<ActionResult<Valera>> SingInMetro(int id)
        {
            return await PerformAction(id, "sing");
        }

        [HttpPost("{id}/sleep")]
        public async Task<ActionResult<Valera>> Sleep(int id)
        {
            return await PerformAction(id, "sleep");
        }

        // Общий метод для выполнения действий
        private async Task<ActionResult<Valera>> PerformAction(int id, string action)
        {
            var valera = await _valeraService.PerformActionAsync(id, action);
            if (valera == null)
                return BadRequest("Invalid action or conditions not met");
            return Ok(valera);
        }
    }
}