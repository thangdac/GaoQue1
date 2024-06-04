using System.ComponentModel.DataAnnotations;

namespace GaoQue.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Hide { get; set; }
    }
}
