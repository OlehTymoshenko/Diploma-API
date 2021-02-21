using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;
using DL.EF.Context;

namespace DL.Repositories
{
    public class RefreshTokensRepository : GenericRepository<RefreshToken>, IRefreshTokensRepository  
    {
        public RefreshTokensRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}
