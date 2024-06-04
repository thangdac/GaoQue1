using System;
using System.ComponentModel.DataAnnotations;

namespace GaoQue.Models
{
    public class GPTData
    {
        public int Id { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
