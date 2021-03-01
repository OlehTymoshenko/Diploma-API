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
            IRefreshTokensRepository refreshTokensRepository)
        {
            _appDbContext = applicationDbContext;
            
            Users = usersRepository;
            Roles = rolesRepository;
            RefreshTokens = refreshTokensRepository;
        }


        public IUsersRepository Users { get; private set; }

        public IRolesRepository Roles { get; private set; }

        public IRefreshTokensRepository RefreshTokens { get; private set; }


        public int SaveChanges()
        {
            return _appDbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
