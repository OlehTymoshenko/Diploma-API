using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DL.Interfaces.Repositories;

namespace DL.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUsersRepository Users { get; }
        public IRolesRepository Roles { get; }
        public IRefreshTokensRepository RefreshTokens { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
