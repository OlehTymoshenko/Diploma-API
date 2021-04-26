using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Models.DataForFiles;

namespace BL.Interfaces.Subdomains.DataForFiles.Services
{
    public interface IScientistService
    {
        Task<IEnumerable<ScientistModel>> GetAllAsync();

        Task<ScientistModel> AddAsync(SaveScientistModel saveScientistModel);

        Task<ScientistModel> UpdateAsync(UpdateScientistModel scientistModel);

        Task<ScientistModel> DeleteAsync(long id);
    }
}
