using DL.EF.Context;
using DL.Entities;
using DL.Interfaces.Repositories;
using DL.Repositories.Abstractions;

namespace DL.Repositories
{
    public class UsersReporitory : GenericRepository<User>,  IUsersRepository
    {
        public UsersReporitory(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}
