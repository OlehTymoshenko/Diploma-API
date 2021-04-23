using DL.EF.Context;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    public class ScientistRepository : GenericRepository<Scientist>, IScientistRepository
    {
        public ScientistRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}
