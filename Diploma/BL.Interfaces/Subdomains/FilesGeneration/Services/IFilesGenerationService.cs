using System.Threading.Tasks;
using DL.Entities.Enums;
using BL.Models.FilesPreparation;

namespace BL.Interfaces.Subdomains.FilesGeneration.Services
{
    public interface IFilesGenerationService
    {
        Task<GeneratedFile> CreateFile(FileType fileType);
    } 
}
