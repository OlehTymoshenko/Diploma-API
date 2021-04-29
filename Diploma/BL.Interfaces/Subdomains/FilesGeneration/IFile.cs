using DL.Entities.Enums;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IFile
    {
        public FileType Type { get; set; }

        public FileFormat Format { get; set; }

        byte[] GetBytes();
    }
}
