namespace vizsgaController.Dtos
{
    public class CommentDTO
    {
        public int commentID { get; set; }
        public int postID { get; set; }
        public string commentcoontent { get; set; }
        public DateTime commentcreated_at { get; set; }
    }
}
