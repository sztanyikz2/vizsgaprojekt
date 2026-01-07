using System.ComponentModel.DataAnnotations;

namespace vizsgaController.Dtos
{
    public class PostDTO
    {
        public int postID { get; set; }
        public int userID { get; set; }
        public int categoryID { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }
    }
}
