using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tasks.Data;
using Tasks.Models;

namespace Tasks.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Models.Task> tasks = new List<Models.Task>();

            try
            {
                using (var context = new EdisonContext())
                {
                    tasks = context.Tasks.ToList();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Error", tasks);
                return RedirectToAction("Error");
            }

            _logger.Log(LogLevel.Information, "Load Success", tasks);
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Models.TaskModel model = new Models.TaskModel();
            model.Priorities = GetEnumList<Priority>();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Models.TaskModel model)
        {
            try
            {
                using (var context = new EdisonContext())
                {
                    context.Tasks.Add(model.Task!);
                    context.Entry(model.Task!).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Create Error", model.Task);
                return RedirectToAction("Error");
            }

            _logger.Log(LogLevel.Information, "Create Success", model.Task);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Models.TaskModel model = new Models.TaskModel();
            model.Priorities = GetEnumList<Priority>();

            try
            {
                using (var context = new EdisonContext())
                {
                    model.Task = context.Tasks.First(t => t.ID.Equals(Id));
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Edit Error", Id);
                return RedirectToAction("Error");
            }

            _logger.Log(LogLevel.Information, "Load Edit Success", model.Task);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Models.TaskModel model)
        {
            model.Priorities = GetEnumList<Priority>();

            try
            {
                using (var context = new EdisonContext())
                {
                    context.Tasks.Update(model.Task!);
                    context.Entry(model.Task!).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Edit Error", model.Task);
                return View(model);
            }

            _logger.Log(LogLevel.Information, "Edit Success", model.Task);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            try
            {
                using (var context = new EdisonContext())
                {
                    var task = context.Tasks.First(t => t.ID.Equals(Id));
                    context.Tasks.Remove(task);
                    context.Entry(task).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Delete Error", Id);
                return RedirectToAction("Error");
            }

            _logger.Log(LogLevel.Information, "Delete Success", Id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
}