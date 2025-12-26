using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Data;
using SahibindenMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // ✅ İlanları LINQ ile değil -> SP ile çekiyoruz
            var pQ = new SqlParameter("@Q", (object?)q ?? DBNull.Value);
            var pCategoryId = new SqlParameter("@CategoryId", (object?)categoryId ?? DBNull.Value);
            var pCityId = new SqlParameter("@CityId", (object?)cityId ?? DBNull.Value);
            var pDistrictId = new SqlParameter("@DistrictId", (object?)districtId ?? DBNull.Value);

            var listings = await _db.Set<VwListingFilterResult>()
                .FromSqlRaw("EXEC dbo.sp_Listings_Filter @Q, @CategoryId, @CityId, @DistrictId",
                    pQ, pCategoryId, pCityId, pDistrictId)
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
