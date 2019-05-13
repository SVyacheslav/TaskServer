using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskServer.Models;

namespace TaskServer.Controllers
{
    public class HomeController : Controller
    {
        TaskContext db;
        public HomeController(TaskContext context)
        {
            db = context;
        }

        
        public IActionResult Index()
        {
            return View("Index");
        }

        // Вывод
        [HttpPost]
        public JsonResult InitialDb()
        {
            return Json(db.Assignments);
        }

        // Добавление
        [HttpPost]
        public JsonResult Create(Assignment assignment)
        {
            db.Assignments.Add(assignment);
            db.SaveChanges();
            return Json(db.Assignments);
        }

        // Установка даты начала выполнения задания
        [HttpPost]
        public JsonResult StartedOn(Assignment assignment)
        {
            assignment.StartedOn = DateTime.Now;
            db.SaveChanges();
            return Json(db.Assignments);
        }

        // Установка времени исполнения задания
        [HttpPost]
        public JsonResult CompletedOn(Assignment assignment)
        {
            assignment.CompletedOn = DateTime.Now; // Это временно!
           // assignment.CompletedOn =  DateTime.Now.Subtract(assignment.StartedOn);
            db.Assignments.Update(assignment);

            db.SaveChanges();
            return Json(db.Assignments.ToList());
        }

        // Удаление задания
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id != null)
            {
                Assignment assignment =  db.Assignments.FirstOrDefault(p => p.Id == id);
                if (assignment != null)
                {
                    db.Assignments.Remove(assignment);
                    db.SaveChanges();
                    
                }
            }
            return Json(db.Assignments);
        }

       
    }
}
