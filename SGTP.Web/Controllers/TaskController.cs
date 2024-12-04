using Microsoft.AspNetCore.Mvc;
using SGTP.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace SGTP.Web.Controllers
{
    public class TasksController : Controller
    {
        private string fileName = @"C:\Users\Jayne\source\repos\SGTP.Web\SGTP.Web\tasks.txt";  
        private static List<TaskModel> tasks = new List<TaskModel>();  
        private static int nextId = 1;

        
        public TasksController()
        {
            LoadTasks();
        }

        
        public IActionResult Index()
        {
            return View(tasks);  
        }

        
        public IActionResult Create()
        {
            var task = new TaskModel();  
            return View(task);  
        }

        
        [HttpPost]
        public IActionResult Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task.Id = nextId++;  
                tasks.Add(task);  
                SaveTasksToFile();  
                return RedirectToAction(nameof(Index)); 
            }
            return View(task);  
        }

        
        [HttpPost]
        public IActionResult Complete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
                SaveTasksToFile();  
            }
            return RedirectToAction(nameof(Index));  
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);  
                SaveTasksToFile();  
            }
            return RedirectToAction(nameof(Index));  
        }

        
        private void LoadTasks()
        {
            if (System.IO.File.Exists(fileName))
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                tasks.Clear();  

                foreach (var line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        string title = parts[0].Trim();
                        string description = parts[1].Trim();

                        DateTime dueDate;
                        bool isValidDate = DateTime.TryParse(parts[2].Trim(), out dueDate);

                        if (!isValidDate)
                        {
                            dueDate = DateTime.MinValue;
                        }

                        bool isCompleted = bool.Parse(parts[3].Trim());

                        int taskId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

                        TaskModel task = new TaskModel { Title = title, Description = description, DueDate = dueDate, IsCompleted = isCompleted, Id = taskId };
                        tasks.Add(task);

                        nextId = tasks.Max(t => t.Id) + 1;
                    }
                }
            }
        }

        
        private void SaveTasksToFile()
        {
            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Title}|{task.Description}|{task.DueDate}|{task.IsCompleted}");
                }
            }
        }
    }
}
