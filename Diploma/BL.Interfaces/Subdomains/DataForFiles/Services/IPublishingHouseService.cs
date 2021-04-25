using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Models.DataForFiles;

namespace BL.Interfaces.Subdomains.DataForFiles.Services
{
    public interface IPublishingHouseService
    {
        Task<IEnumerable<PublishingHouseModel>> GetAllAsync();

        Task<PublishingHouseModel> AddAsync(SavePublishingHouseModel savePublishingHouseModel);

        Task<PublishingHouseModel> UpdateAsync(PublishingHouseModel publishingHouseModel);

        Task<PublishingHouseModel> DeleteAsync(long id);
    }
}
