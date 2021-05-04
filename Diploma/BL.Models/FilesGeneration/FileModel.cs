using DL.Entities.Enums;

namespace BL.Models.FilesGeneration
{
    public class FileModel
    {
        public FileType Type { get; set; }

        public FileFormat Format { get; set; }

        public byte[] FileAsBytes { get; set; }
    }
}
