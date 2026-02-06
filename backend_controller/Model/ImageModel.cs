﻿using Microsoft.EntityFrameworkCore;
using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public class ImageModel : IImageModel
    {
        private readonly NewsDbContext _context;
        public ImageModel(NewsDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveImageAsync(int postid, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //something exception;
            }
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var img = new Image
            {
                PostId = postid,
                ImageContent = ms.ToArray(),
                ContentType = file.ContentType
            };

            _context.Images.Add(img);
            await _context.SaveChangesAsync();

            return img.ImageId;
        }

        public byte[] GetImage(int id)
        {
            var image = _context.Find<Image>([id]);
            if (image == null)
            {
                throw new KeyNotFoundException();
            }
            return image.ImageContent;
        }

    }
}
