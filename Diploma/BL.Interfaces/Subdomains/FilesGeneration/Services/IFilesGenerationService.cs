using BL.Models.FilesGeneration;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IFilesGenerationService
    {
        Task<CreatedFileModel> CreateNotesOfAuthorsFileAsync(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel);
    } 
}
