using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Data;
using SahibindenMvc.Models;

namespace SahibindenMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly SahibindenDbContext _db;

        public HomeController(SahibindenDbContext db)
        {
            _db = db;
        }

        // /?q=...&categoryId=...&cityId=...&districtId=...
        public async Task<IActionResult> Index(string? q, int? categoryId, int? cityId, int? districtId)
        {
            // ✅ VIEW üzerinden sorgu (Include YOK)
            var query = _db.VwListings.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x => x.Title.Contains(q));

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            // ✅ Konum filtresi: VIEW alanlarıyla
            if (districtId.HasValue)
                query = query.Where(x => x.DistrictId == districtId.Value);
            else if (cityId.HasValue)
                query = query.Where(x => x.CityId == cityId.Value);

            // ✅ Tek seferde liste al (listings 1 kere tanımlanır)
            var listings = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(50)
                .ToListAsync();

            // Kategori ağacı (sol menü için)
            var allCats = await _db.Categories
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.CategoryName)
                .ToListAsync();

            ViewBag.RootCategories = allCats.Where(x => x.ParentId == null).ToList();
            ViewBag.AllCategories = allCats;

            // Şehirler (il)
            ViewBag.Cities = await _db.Locations
                .Where(l => l.LocationType == 1)
                .OrderBy(l => l.LocationName)
                .ToListAsync();

            // İlçeler (seçilen ile göre)
            if (cityId.HasValue)
            {
                ViewBag.Districts = await _db.Locations
                    .Where(l => l.ParentId == cityId.Value && l.LocationType == 2)
                    .OrderBy(l => l.LocationName)
                    .ToListAsync();
            }
            else
            {
                ViewBag.Districts = new List<Location>();
            }

            // Seçimler
            ViewBag.Q = q;
            ViewBag.CategoryId = categoryId;
            ViewBag.CityId = cityId;
            ViewBag.DistrictId = districtId;

            return View(listings);
        }
    }
}
