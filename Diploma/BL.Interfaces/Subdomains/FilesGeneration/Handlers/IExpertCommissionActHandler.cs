using BL.Models.FilesGeneration;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IExpertCommissionActHandler : IFileHandler
    {
        Task<FileModel> CreateFileAsync(SaveExpertCommissionActModel saveExpertCommissionActModel);
    }
}
