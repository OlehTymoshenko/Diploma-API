using System;
using System.IO;
using System.Threading.Tasks;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal class TemplateLoader
    {
        internal const string DEFAULT_TEMPLATES_FOLDER = @"Templates";

        internal string BaseDirectory { get; init; }

        internal string TemplatesFolder { get; init; } 


        internal TemplateLoader(string templatesFolder = null, string baseDirectory = null)
        {
            BaseDirectory = baseDirectory ?? AppContext.BaseDirectory;
            TemplatesFolder = templatesFolder ?? DEFAULT_TEMPLATES_FOLDER;
        }


        internal async Task<MemoryStream> LoadTemplateAsync(string fileName)
        {
            string pathToFile = Path.Combine(BaseDirectory, TemplatesFolder, fileName);

            if (!File.Exists(pathToFile)) throw new ArgumentException($"Path to file is invalid. \nPath: {pathToFile}");

            var fileBytes = await File.ReadAllBytesAsync(pathToFile);
            
            var memStream = new MemoryStream();
            
            await memStream.WriteAsync(fileBytes);
          
            return memStream;
        }
    }
}
