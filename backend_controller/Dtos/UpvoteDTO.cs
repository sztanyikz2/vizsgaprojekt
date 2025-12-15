namespace vizsgaController.Dtos
{
    public class UpvoteDTO
    {
        public int upvoteID { get; set; }
        public int postID { get; set; }
        public int userID { get; set; }
        public DateTime upvotecreated_at { get; set; }
    }
}
