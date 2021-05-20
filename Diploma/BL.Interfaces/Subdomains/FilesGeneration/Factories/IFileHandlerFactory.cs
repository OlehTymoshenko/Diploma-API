using DL.Entities.Enums;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IFileHandlerFactory
    {
        public INotesOfAuthorsHandler GetNotesOfAuthorsHandler(FileFormat fileFormat);

        public IExpertCommissionActHandler GetExpertCommissionActHandler(FileFormat fileFormat);
    }
}
