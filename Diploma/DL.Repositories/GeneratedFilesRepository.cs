using DL.EF.Context;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    public class GeneratedFilesRepository : GenericRepository<GeneratedFile>, IGeneratedFileRepository
    {
        public GeneratedFilesRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}