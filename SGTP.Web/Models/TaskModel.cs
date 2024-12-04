using System;

namespace SGTP.Web.Models
{
    public class TaskModel
    {
        public static int Counter = 1; 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public TaskModel()
        {
            Id = Counter++;
        }
    }
}