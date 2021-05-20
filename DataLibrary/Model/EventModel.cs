using System;
using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Model
{
    public class EventModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "To long text")]
        public string EventName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "To long text")]
        public string EventDesc { get; set; }

        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
    }
}