using System;
using System.ComponentModel.DataAnnotations;

namespace projectDesktop.DTOs
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
        public DateTime created_at { get; set; }

    }
}
