using DL.Entities.Enums;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IFileHandler
    {
        public FileType Type { get; }

        public FileFormat Format { get; }
    }
}
