using System.ComponentModel.DataAnnotations;

namespace BusinessService.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        // YOB= year of birth 
        public int YOB { get; set; }
    }
}
