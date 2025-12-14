using gestionpiece3.DTO;
using gestionpiece3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace gestionpiece3.Controllers
{

    [Route("api/[controller]")]
    public class PiecesController : Controller
    {
        private readonly SymfonyAuthService _authService;
        private readonly PieceService _pieceService;

        public PiecesController(SymfonyAuthService authService, PieceService pieceService)
        {
            _authService = authService;
            _pieceService = pieceService;
        }

        public IActionResult Add() => View();

      
       
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddPieceDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.name))
                return BadRequest(new { success = false, message = "Le nom est obligatoire" });

            var token = Request.Headers["X-AUTH-TOKEN"].ToString();
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { success = false, message = "Token manquant" });

            var userId = await _authService.GetUserIdAsync(token);
            if (userId == null)
                return Unauthorized(new { success = false, message = "Token invalide ou expiré" });

            await _pieceService.AddPieceAsync(dto.name, userId.Value);
            return Ok(new { success = true, message = "Pièce ajoutée avec succès" });
        }


    }
}
