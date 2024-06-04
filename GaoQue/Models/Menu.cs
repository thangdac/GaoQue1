using System.ComponentModel.DataAnnotations;

namespace GaoQue.Models
{
    public class Menu
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; } 
        public string Link { get; set; }  
        public int Hide { get; set; }   
    }
}
