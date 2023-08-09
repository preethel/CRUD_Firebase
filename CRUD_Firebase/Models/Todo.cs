using System.ComponentModel.DataAnnotations;

namespace CRUD_Firebase.Models
{
    public class Todo
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        //public string Description { get; set; } = string.Empty;
        //public string StartedTime { get; set; } = string.Empty;
        //public string EndTime { get; set; } = string.Empty;
        //public int Status { get; set; }
    }
}
