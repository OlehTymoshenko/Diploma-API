using DL.EF.Context;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    public class DegreRepository : GenericRepository<Degree>, IDegreeRepository
    {
        public DegreRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}