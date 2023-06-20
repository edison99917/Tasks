using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Edison.Data;
using Edison.Models;

namespace Edison.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        private readonly ILogger<AssetController> _logger;

        public AssetController(ILogger<AssetController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Models.Asset> assets = new List<Models.Asset>();

            try
            {
                using (var context = new EdisonContext())
                {
                    assets = (from c in context.Asset
                              select new Asset
                              {
                                  ID = c.ID,
                                  AssetCode = c.AssetCode,
                                  AssetName = c.AssetName,
                                  AssetCategory = context.AssetCategory.Where(x => x.ID.Equals(c.AssetCategoryID)).First(),
                                  Department = context.Department.Where(x => x.ID.Equals(c.DepartmentID)).First(),
                                  PurchaseDate = c.PurchaseDate,
                                  Price = c.Price,
                                  SupplierName = c.SupplierName
                              }).ToList();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Error", assets);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Load Success", assets);
            return View(assets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Models.AssetModel model = new Models.AssetModel();
            using (var context = new EdisonContext())
            {
                model.AssetCategories = context.AssetCategory.ToList();
                model.Departments = context.Department.ToList();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Models.AssetModel model)
        {
            try
            {
                using (var context = new EdisonContext())
                {
                    context.Asset.Add(model.Asset!);
                    context.Entry(model.Asset!).State = EntityState.Added;
                    context.Entry(model.Asset!.AssetCategory!).State = EntityState.Unchanged;
                    context.Entry(model.Asset!.Department!).State = EntityState.Unchanged;

                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Create Error", model.Asset);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Create Success", model.Asset);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Models.AssetModel model = new Models.AssetModel();

            try
            {
                using (var context = new EdisonContext())
                {
                    model.Asset = context.Asset.First(a => a.ID.Equals(Id));
                    model.AssetCategories = context.AssetCategory.ToList();
                    model.Departments = context.Department.ToList();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Edit Error", Id);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Load Edit Success", model.Asset);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Models.AssetModel model)
        {
            using (var context = new EdisonContext())
            {
                try
                {
                    context.Asset.Update(model.Asset!);
                    context.Entry(model.Asset!).State = EntityState.Modified;
                    context.Entry(model.Asset!.AssetCategory!).State = EntityState.Unchanged;
                    context.Entry(model.Asset!.Department!).State = EntityState.Unchanged;
                    context.SaveChanges();
                }
                catch
                {
                    model.AssetCategories = context.AssetCategory.ToList();
                    model.Departments = context.Department.ToList();

                    _logger.Log(LogLevel.Error, "Edit Error", model.Asset);
                    return View(model);
                }
            }

            _logger.Log(LogLevel.Information, "Edit Success", model.Asset);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                using (var context = new EdisonContext())
                {
                    var asset = context.Asset.First(a => a.ID.Equals(Id));
                    context.Asset.Remove(asset);
                    context.Entry(asset).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Delete Error", Id);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Delete Success", Id);
            return RedirectToAction("Index");
        }
    }
}
