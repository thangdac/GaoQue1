using System.ComponentModel.DataAnnotations;

namespace GaoQue.Models
{
    public class Banner
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }    
        public string Description { get; set; }     
        public string? Imageurl { get; set;}
    }
}
