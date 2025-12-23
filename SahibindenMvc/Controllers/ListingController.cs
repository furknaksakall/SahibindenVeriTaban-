using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Data;

namespace SahibindenMvc.Controllers
{
    public class ListingController : Controller
    {
        private readonly SahibindenDbContext _db;

        public ListingController(SahibindenDbContext db)
        {
            _db = db;
        }

        [HttpGet("/ilan/{id:int}")]
        public async Task<IActionResult> Detail(int id)
        {
            var listing = await _db.Listings
                .Include(x => x.Category)
                .Include(x => x.Location)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ListingId == id);

            if (listing == null) return NotFound();

            return View(listing);
        }
    }
}
