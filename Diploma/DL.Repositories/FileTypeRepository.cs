﻿using DL.EF.Context;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    public class FileTypeRepository : GenericRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}