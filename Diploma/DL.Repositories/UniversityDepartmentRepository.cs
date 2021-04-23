using DL.EF.Context;
using DL.Entities;
using DL.Interfaces.Repositories;
using DL.Repositories.Abstractions;

namespace DL.Repositories
{
    public class UniversityDepartmentRepository : GenericRepository<UniversityDepartment>, IUniversityDepartmentRepository
    {
        public UniversityDepartmentRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}
