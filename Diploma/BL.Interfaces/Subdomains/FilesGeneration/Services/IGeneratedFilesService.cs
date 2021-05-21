using BL.Models.FilesGeneration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration.Services
{
    public interface IGeneratedFilesService
    {
        Task<IEnumerable<DescriptionOfGeneratedFile>> GetUserGeneratedFiles(IEnumerable<Claim> userClaims);
    }
}
