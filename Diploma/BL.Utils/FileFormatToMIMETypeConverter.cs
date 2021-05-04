using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.Entities.Enums;

namespace BL.Utils
{
    public static class FileFormatToMIMETypeConverter
    {
        public const string DOCX_MIME_TYPE = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public static string FileFormatToMIME(FileFormat fileFormat)
        {
            return fileFormat switch
            {
                FileFormat.DOCX => DOCX_MIME_TYPE,
                _ => throw new ArgumentException("Unknown type")
            };
        }
    }
}
