using BL.Models.FilesGeneration;

namespace BL.Interfaces.Subdomains.FilesGeneration.Core
{
    public interface INoteOfAuthors : IFile
    {
        void CreateFile(SaveNoteOfAuthorsModel saveModel);
    }
}
