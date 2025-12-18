namespace vizsgaController.Model
{
    public interface IImageModel
    {
        public async Task<int> SaveImageAsync(int userId, IFormFile file);
        public byte[] GetImage(int id);
    }
}
