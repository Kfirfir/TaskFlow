using System.ComponentModel.DataAnnotations;    
namespace TaskFlow.Models
{
    public class Taskk
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        public string TaskkName { get; set; }

        [Required(ErrorMessage = "Task content is required.")]
        public string TaskkContent { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Taskk()
        {
                
        }

    }
}
