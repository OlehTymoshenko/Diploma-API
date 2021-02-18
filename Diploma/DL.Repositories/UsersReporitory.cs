using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities;
using DL.Interfaces.Repositories;
using DL.Repositories.Abstractions;

namespace DL.Repositories
{
    class UsersReporitory : GenericRepository<User>,  IUsersRepository
    {

    }
}
