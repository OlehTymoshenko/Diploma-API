using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    class RefreshTokensRepository : GenericRepository<RefreshToken>, IRefreshTokensRepository  
    {

    }
}
