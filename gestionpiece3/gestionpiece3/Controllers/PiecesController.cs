using gestionpiece3.Data;
using gestionpiece3.DTO;
using gestionpiece3.Models;
using gestionpiece3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace gestionpiece3.Controllers
{

    [Route("api/[controller]")]
    public class PiecesController : Controller
    {
        private readonly SymfonyAuthService _authService;
        private readonly PieceService _pieceService;
        private readonly AppDbContext _context;

        public PiecesController(SymfonyAuthService authService, PieceService pieceService, AppDbContext context)
        {
            _authService = authService;
            _pieceService = pieceService;
            _context = context;
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

            var idPiece = await _pieceService.AddPieceAsync(dto.name, userId.Value);
            return Ok(new { success = true, message = "Pièce ajoutée avec succès" , id=idPiece });
        }
        [HttpGet("with-equipements/{userId}")]
        public async Task<IActionResult> GetPiecesWithEquipements(int userId)
        {
            var result = await (from p in _context.piece
                                join e in _context.equipement on p.id equals e.id into eqs
                                where p.user_id == userId
                                select new
                                {
                                    p.id,
                                    p.name,
                                    Equipements = eqs.Select(e => new
                                    {
                                        e.nom,
                                        e.description,
                                        e.etat
                                    }).ToList()
                                }).ToListAsync();

            return Ok(result);
        }



    }
}
