using System;
using System.Collections.Generic;
using System.Text;
using DL.EF.Context;
using DL.Entities;
using DL.Interfaces.Repositories;
using DL.Repositories.Abstractions;

namespace DL.Repositories
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {
        public RolesRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}
