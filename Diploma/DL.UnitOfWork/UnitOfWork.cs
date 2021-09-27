using System.Threading.Tasks;
using DL.Interfaces.Repositories;
using DL.Interfaces.UnitOfWork;
using DL.EF.Context;
using System;

namespace DL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbContext;

        public UnitOfWork(
            ApplicationDbContext applicationDbContext,
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IRefreshTokensRepository refreshTokensRepository,
            IDegreeRepository degreeRepository,
            IGeneratedFileRepository generatedFileRepository,
            IPublishingHouseRepository publishingHouseRepository,
            IScientistRepository scientistRepository,
            IUniversityDepartmentRepository universityDepartmentRepository)
        {
            _appDbContext = applicationDbContext;
            
            Users = usersRepository;
            Roles = rolesRepository;
            RefreshTokens = refreshTokensRepository;
            Degrees = degreeRepository;
            GeneratedFiles = generatedFileRepository;
            PublishingHouses = publishingHouseRepository;
            Scientists = scientistRepository;
            UniversityDepartments = universityDepartmentRepository;
        }


        public IUsersRepository Users { get; init; }

        public IRolesRepository Roles { get; init; }

        public IRefreshTokensRepository RefreshTokens { get; init; }

        public IDegreeRepository Degrees { get; init; }

        public IGeneratedFileRepository GeneratedFiles { get; init; }

        public IPublishingHouseRepository PublishingHouses { get; init; }

        public IScientistRepository Scientists { get; init; }

        public IUniversityDepartmentRepository UniversityDepartments{ get; init; }


        public int SaveChanges()
        {
            return _appDbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
