using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DL.Entities;
using DL.Interfaces.Repositories.Abstractions;

namespace DL.Interfaces.Repositories
{
    public interface IUsersRepository : IGenericRepository<User>
    {
    }
}
