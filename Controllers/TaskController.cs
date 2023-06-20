using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Edison.Data;

namespace Edison.Controllers
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
                    tasks = context.Task.ToList();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Error", tasks);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Load Success", tasks);
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Models.TaskModel model = new Models.TaskModel();
            model.Priorities = GetEnumList<Models.Priority>();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Models.TaskModel model)
        {
            try
            {
                using (var context = new EdisonContext())
                {
                    context.Task.Add(model.Task!);
                    context.Entry(model.Task!).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Create Error", model.Task);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Create Success", model.Task);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Models.TaskModel model = new Models.TaskModel();
            model.Priorities = GetEnumList<Models.Priority>();

            try
            {
                using (var context = new EdisonContext())
                {
                    model.Task = context.Task.First(t => t.ID.Equals(Id));
                }
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Load Edit Error", Id);
                return RedirectToAction("Error", "Home");
            }

            _logger.Log(LogLevel.Information, "Load Edit Success", model.Task);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Models.TaskModel model)
        {
            using (var context = new EdisonContext())
            {
                try
                {
                    context.Task.Update(model.Task!);
                    context.Entry(model.Task!).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch
                {
                    model.Priorities = GetEnumList<Models.Priority>();

                    _logger.Log(LogLevel.Error, "Edit Error", model.Task);
                    return View(model);
                }
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
                    var task = context.Task.First(t => t.ID.Equals(Id));
                    context.Task.Remove(task);
                    context.Entry(task).State = EntityState.Deleted;
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

        private static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }
    }
}