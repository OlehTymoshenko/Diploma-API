using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Models.DataForFiles;


namespace BL.Interfaces.Subdomains.DataForFiles.Services
{
    public interface IDegreeService
    {
        Task<IEnumerable<DegreeModel>> GetAllAsync();

        Task<DegreeModel> AddAsync(SaveDegreeModel saveDegreeModel);

        Task<DegreeModel> UpdateAsync(DegreeModel degreeModel);

        Task<DegreeModel> DeleteAsync(long id);
    }
}
