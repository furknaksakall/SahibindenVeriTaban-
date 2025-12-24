using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Data;
using SahibindenMvc.Models;

namespace SahibindenMvc.Controllers
{
    public class ListingController : Controller
    {
        private readonly SahibindenDbContext _db;

        public ListingController(SahibindenDbContext db)
        {
            _db = db;
        }

        // ✅ İLAN DETAY
        [HttpGet("/ilan/{id:int}")]
        public async Task<IActionResult> Detail(int id)
        {
            var listing = await _db.Listings
                .Include(x => x.Category)
                .Include(x => x.Location)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ListingId == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        // ✅ İLAN EKLE (FORM)
        [HttpGet("/ilan/ekle")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _db.Users.OrderBy(x => x.FullName).ToListAsync();
            ViewBag.Categories = await _db.Categories.OrderBy(x => x.CategoryName).ToListAsync();
            ViewBag.Locations = await _db.Locations.OrderBy(x => x.LocationName).ToListAsync();

            return View(new ListingCreateVm());
        }

        // ✅ İLAN EKLE (SP ÇALIŞTIR)
        [HttpPost("/ilan/ekle")]
        public async Task<IActionResult> Create(ListingCreateVm vm)
        {
            ViewBag.Users = await _db.Users.OrderBy(x => x.FullName).ToListAsync();
            ViewBag.Categories = await _db.Categories.OrderBy(x => x.CategoryName).ToListAsync();
            ViewBag.Locations = await _db.Locations.OrderBy(x => x.LocationName).ToListAsync();

            if (!ModelState.IsValid)
                return View(vm);

            var pUserId = new SqlParameter("@UserId", vm.UserId);
            var pCategoryId = new SqlParameter("@CategoryId", vm.CategoryId);
            var pLocationId = new SqlParameter("@LocationId", vm.LocationId);
            var pTitle = new SqlParameter("@Title", vm.Title);
            var pDesc = new SqlParameter("@Description", vm.Description);
            var pPrice = new SqlParameter("@Price", vm.Price);
            var pCurrency = new SqlParameter("@Currency", vm.Currency);

            var newIdRow = await _db.Set<SpNewIdResult>()
                .FromSqlRaw(
                    "EXEC dbo.sp_Listing_Add @UserId, @CategoryId, @LocationId, @Title, @Description, @Price, @Currency",
                    pUserId, pCategoryId, pLocationId, pTitle, pDesc, pPrice, pCurrency
                )
                .ToListAsync();

            var newIdDecimal = newIdRow.FirstOrDefault()?.NewListingId ?? 0m;
            var newId = (int)newIdDecimal;

            return Redirect($"/ilan/{newId}");
        }

        // ✅ İLAN DÜZENLE (FORM)
        [HttpGet("/ilan/{id:int}/duzenle")]
        public async Task<IActionResult> Edit(int id)
        {
            var listing = await _db.Listings.FirstOrDefaultAsync(x => x.ListingId == id);
            if (listing == null) return NotFound();

            ViewBag.Categories = await _db.Categories.OrderBy(x => x.CategoryName).ToListAsync();
            ViewBag.Locations = await _db.Locations.OrderBy(x => x.LocationName).ToListAsync();

            var vm = new ListingEditVm
            {
                ListingId = listing.ListingId,
                CategoryId = listing.CategoryId,
                LocationId = listing.LocationId,
                Title = listing.Title,
                Description = listing.Description ?? "",
                Price = listing.Price,
                Currency = listing.Currency ?? "TRY",
                IsActive = listing.IsActive
            };

            return View(vm);
        }

        // ✅ İLAN DÜZENLE (SP ÇALIŞTIR)
        [HttpPost("/ilan/{id:int}/duzenle")]
        public async Task<IActionResult> Edit(int id, ListingEditVm vm)
        {
            ViewBag.Categories = await _db.Categories.OrderBy(x => x.CategoryName).ToListAsync();
            ViewBag.Locations = await _db.Locations.OrderBy(x => x.LocationName).ToListAsync();

            if (id != vm.ListingId) return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            var pListingId = new SqlParameter("@ListingId", vm.ListingId);
            var pCategoryId = new SqlParameter("@CategoryId", vm.CategoryId);
            var pLocationId = new SqlParameter("@LocationId", vm.LocationId);
            var pTitle = new SqlParameter("@Title", vm.Title);
            var pDesc = new SqlParameter("@Description", vm.Description);
            var pPrice = new SqlParameter("@Price", vm.Price);
            var pCurrency = new SqlParameter("@Currency", vm.Currency);
            var pIsActive = new SqlParameter("@IsActive", vm.IsActive);

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Listing_Update @ListingId, @CategoryId, @LocationId, @Title, @Description, @Price, @Currency, @IsActive",
                pListingId, pCategoryId, pLocationId, pTitle, pDesc, pPrice, pCurrency, pIsActive
            );

            return Redirect($"/ilan/{vm.ListingId}");
        }

        // ✅ İLAN YAYINDAN KALDIR (SOFT DELETE) - SP ÇALIŞTIR
        [HttpPost("/ilan/{id:int}/kaldir")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var pId = new SqlParameter("@ListingId", id);

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_Listing_Deactivate @ListingId",
                pId
            );

            return Redirect("/");
        }
    }
}
