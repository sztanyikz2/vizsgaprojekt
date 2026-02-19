using System.ComponentModel.DataAnnotations;

namespace vizsgaController.Dtos
{
    public class ReportDTO
    {
        public int postID { get; set; }
        public int userID { get; set; }
        [Required]
        public string reportreason { get; set; }
        public DateTime reportcreated_at { get; set; }
    }
}
