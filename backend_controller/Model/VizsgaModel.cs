using vizsgaController.Dtos;
using vizsgaController.Persistence;

namespace vizsgaController.Model
{
    public class VizsgaModel
    {
        private readonly VizsgaDbContext _context;
        public VizsgaModel(VizsgaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserDTO> GetUserNamesBySearch(string name)
        {
            if (name != null)
            {
                return _context.users.Where(x => x.username.ToLower() == name.ToLower()).Select(x => new UserDTO
                {
                    username = x.username,
                    useremail = x.useremail,
                    userpassword = x.userpassword,
                });


            }
            throw new InvalidDataException("Töltsd ki a keresőmezőt, pretty please");
        }


        public IEnumerable<PostDTO> GetPostsBySearch(string title)
        {
            if (title != null)
            {
                return _context.posts.Where(x => x.title.ToLower().Contains(title.ToLower())).Select(x => new PostDTO
                {
                    title = x.title,
                    content = x.content,
                    created_at = x.created_at,
                    deleted_at = x.deleted_at,
                });
            }
            throw new InvalidDataException("Töltsd ki a keresőmezőt, pretty please");
        }



    }
}
