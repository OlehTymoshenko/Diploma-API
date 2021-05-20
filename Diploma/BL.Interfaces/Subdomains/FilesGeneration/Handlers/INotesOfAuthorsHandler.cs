using BL.Models.FilesGeneration;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface INotesOfAuthorsHandler : IFileHandler
    {
        Task<FileModel> CreateFileAsync(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel);
    }
}
