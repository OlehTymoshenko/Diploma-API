using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DL.Interfaces.Repositories;

namespace DL.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUsersRepository Users { get; init; }

        public IRolesRepository Roles { get; init; }

        public IRefreshTokensRepository RefreshTokens { get; init; }

        public IDegreeRepository Degrees { get; init; }

        public IGeneratedFileRepository GeneratedFiles { get; init; }

        public IPublishingHouseRepository PublishingHouses { get; init; }

        public IScientistRepository Scientists { get; init; }

        public IUniversityDepartmentRepository UniversityDepartments { get; init; }


        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
