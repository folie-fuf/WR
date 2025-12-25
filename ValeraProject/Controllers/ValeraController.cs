using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValeraProject.DTOs;
using ValeraProject.Services;

namespace ValeraProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ValeraDto>>> GetAllValeras()
        {
            var valeras = await _valeraService.GetAllValerasAsync(User);
            var valeraDtos = valeras.Select(v => new ValeraDto
            {
                Id = v.Id,
                Health = v.Health,
                Mana = v.Mana,
                Cheerfulness = v.Cheerfulness,
                Fatigue = v.Fatigue,
                Money = v.Money
            }).ToList();
            
            return Ok(valeraDtos);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<ValeraDto>>> GetMyValeras()
        {
            var valeras = await _valeraService.GetMyValerasAsync(User);
            var valeraDtos = valeras.Select(v => new ValeraDto
            {
                Id = v.Id,
                Health = v.Health,
                Mana = v.Mana,
                Cheerfulness = v.Cheerfulness,
                Fatigue = v.Fatigue,
                Money = v.Money,
                UserId = v.UserId
            }).ToList();
            
            return Ok(valeraDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValeraDto>> GetValeraById(int id)
        {
            var valera = await _valeraService.GetValeraByIdAsync(id, User);
            if (valera == null)
                return NotFound();

            return Ok(new ValeraDto
            {
                Id = valera.Id,
                Health = valera.Health,
                Mana = valera.Mana,
                Cheerfulness = valera.Cheerfulness,
                Fatigue = valera.Fatigue,
                Money = valera.Money,
                UserId = valera.UserId,
                User = valera.User != null ? new 
                {
                    Id = valera.User.Id,
                    Username = valera.User.Username,
                    Email = valera.User.Email
                } : null
            });
        }

        [HttpPost]
        public async Task<ActionResult<ValeraDto>> CreateValera(CreateValeraDto createValeraDto)
        {
            var valera = new Models.Valera
            {
                Health = createValeraDto.Health,
                Mana = createValeraDto.Mana,
                Cheerfulness = createValeraDto.Cheerfulness,
                Fatigue = createValeraDto.Fatigue,
                Money = createValeraDto.Money
            };

            var createdValera = await _valeraService.CreateValeraAsync(valera, User);
            
            return CreatedAtAction(nameof(GetValeraById), new { id = createdValera.Id }, new ValeraDto
            {
                Id = createdValera.Id,
                Health = createdValera.Health,
                Mana = createdValera.Mana,
                Cheerfulness = createdValera.Cheerfulness,
                Fatigue = createdValera.Fatigue,
                Money = createdValera.Money
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValera(int id)
        {
            var result = await _valeraService.DeleteValeraAsync(id, User);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/work")]
        public async Task<ActionResult<ValeraDto>> Work(int id)
        {
            return await PerformAction(id, "work");
        }

        [HttpPost("{id}/nature")]
        public async Task<ActionResult<ValeraDto>> Nature(int id)
        {
            return await PerformAction(id, "nature");
        }

        [HttpPost("{id}/wine")]
        public async Task<ActionResult<ValeraDto>> Wine(int id)
        {
            return await PerformAction(id, "wine");
        }

        [HttpPost("{id}/bar")]
        public async Task<ActionResult<ValeraDto>> Bar(int id)
        {
            return await PerformAction(id, "bar");
        }

        [HttpPost("{id}/marginals")]
        public async Task<ActionResult<ValeraDto>> Marginals(int id)
        {
            return await PerformAction(id, "marginals");
        }

        [HttpPost("{id}/sing")]
        public async Task<ActionResult<ValeraDto>> Sing(int id)
        {
            return await PerformAction(id, "sing");
        }

        [HttpPost("{id}/sleep")]
        public async Task<ActionResult<ValeraDto>> Sleep(int id)
        {
            return await PerformAction(id, "sleep");
        }

        private async Task<ActionResult<ValeraDto>> PerformAction(int id, string action)
        {
            var valera = await _valeraService.PerformActionAsync(id, action, User);
            if (valera == null)
                return BadRequest("Action failed or not allowed");

            return Ok(new ValeraDto
            {
                Id = valera.Id,
                Health = valera.Health,
                Mana = valera.Mana,
                Cheerfulness = valera.Cheerfulness,
                Fatigue = valera.Fatigue,
                Money = valera.Money
            });
        }
    }
}