using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Models.DataForFiles;

namespace BL.Interfaces.Subdomains.DataForFiles.Services
{
    public interface IUniversityDepartmentService
    {
        Task<IEnumerable<UniversityDepartmentModel>> GetAllAsync();

        Task<UniversityDepartmentModel> AddAsync(SaveUniversityDepartmentModel saveUniversityDepartmentModel);

        Task<UniversityDepartmentModel> UpdateAsync(UniversityDepartmentModel universityDepartmentModel);

        Task<UniversityDepartmentModel> DeleteAsync(long id);
    }
}
