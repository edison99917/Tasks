using System.ComponentModel.DataAnnotations;

namespace Tasks.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public bool IsCompleted { get; set; }
    }

    public enum Priority
    {
        Low,
        Normal,
        High,
        Urgent
    }
}
