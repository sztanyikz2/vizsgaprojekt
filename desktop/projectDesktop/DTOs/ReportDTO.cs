using System;
using System.ComponentModel.DataAnnotations;

namespace projectDesktop.DTOs
{
    public class ReportDTO
    {
        public int reportID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        [Required]
        public string reportreason { get; set; }
        public DateTime reportcreated_at { get; set; }
        public string reportstatus { get; set; }
    }
}
