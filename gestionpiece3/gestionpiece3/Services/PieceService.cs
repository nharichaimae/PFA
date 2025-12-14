using gestionpiece3.Data;
using gestionpiece3.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionpiece3.Services
{
    public class PieceService
    {

        private readonly AppDbContext _context;

        public PieceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPieceAsync(string name, int userId)
        {
            var piece = new Piece
            {
                name = name,
                user_id = userId
            };

            _context.piece.Add(piece);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            return true;
        }
    }
}
