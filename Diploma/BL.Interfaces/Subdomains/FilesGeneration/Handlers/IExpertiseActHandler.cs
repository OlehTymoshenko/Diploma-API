using System.Threading.Tasks;
using BL.Models.FilesGeneration;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IExpertiseActHandler
    {
        Task<FileModel> CreateFileAsync(SaveExpertiseActModel saveExpertiseActModel);
    }
}
