using gestionpiece3.Data;
using gestionpiece3.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestionpiece3.Controllers
{
    [Route("/api/equipement")]
    public class EquipementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipementController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddEquipement([FromBody] equipementModel equipement)
        {
            equipement.etat = "OFF";
            equipement.id_equipement = 0;
            _context.equipement.Add(equipement);
            await _context.SaveChangesAsync();
            return Ok(equipement);
        }
    }
}
