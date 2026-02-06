namespace vizsgaController.Dtos;

public class VoteDTO
{
    public bool isPositive { get; set; }
    public int userId { get; set; }
    public int postId { get; set; }
}