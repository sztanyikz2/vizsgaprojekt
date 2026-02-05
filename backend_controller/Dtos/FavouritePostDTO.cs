namespace vizsgaController.Dtos;

public class FavouritePostDTO
{
    public int userId { get; set; }
    public int postId { get; set; }
    public bool addTo {get; set;}
}