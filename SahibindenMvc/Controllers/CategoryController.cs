using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Data;
using SahibindenMvc.Models;

namespace SahibindenMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly SahibindenDbContext _db;

        public CategoryController(SahibindenDbContext db)
        {
            _db = db;
        }

        [HttpGet("/kategori/ekle")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();

            return View(new CategoryCreateVm());
        }

        [HttpPost("/kategori/ekle")]
        public async Task<IActionResult> Create(CategoryCreateVm vm)
        {
            ViewBag.Categories = await _db.Categories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();

            if (!ModelState.IsValid)
                return View(vm);

            var pParentId = new SqlParameter("@ParentId", (object?)vm.ParentId ?? DBNull.Value);
            var pName = new SqlParameter("@CategoryName", vm.CategoryName);
            var pSort = new SqlParameter("@SortOrder", vm.SortOrder);

            var res = await _db.Set<SpCategoryIdResult>()
                .FromSqlRaw("EXEC dbo.sp_Category_Add @ParentId, @CategoryName, @SortOrder",
                    pParentId, pName, pSort)
                .ToListAsync();

            return Redirect("/");
        }
    }
}
