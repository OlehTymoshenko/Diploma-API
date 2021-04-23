using DL.EF.Context;
using DL.Entities;
using DL.Repositories.Abstractions;
using DL.Interfaces.Repositories;

namespace DL.Repositories
{
    public class PublishingHouseRepository : GenericRepository<PublishingHouse>, IPublishingHouseRepository
    {
        public PublishingHouseRepository(ApplicationDbContext appDbContext) : base(appDbContext) { }
    }
}

