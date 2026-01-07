namespace vizsgaController.Model
{
    public interface IImageModel
    {
        public Task<int> SaveImageAsync(int userId, IFormFile file);
        public byte[] GetImage(int id);
    }
}
