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

        
    }
}
