namespace vizsgaController.Dtos
{
    public class DownvoteDTO
    {
        public int downvoteID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        public DateTime downvotecreated_at { get; set; }
    }
}
