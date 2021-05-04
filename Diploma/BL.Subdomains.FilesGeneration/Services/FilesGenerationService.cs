using AutoMapper;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using DL.Entities.Enums;
using System;
using System.Threading.Tasks;

namespace BL.Subdomains.FilesGeneration
{
    public class FilesGenerationService : IFilesGenerationService
    {
        readonly IFileHandlerFactory _fileHandlerFactory;
        readonly IMapper _mapper;

        public FilesGenerationService(IFileHandlerFactory fileHandlerFactory, 
            IMapper mapper)
        {
            _fileHandlerFactory = fileHandlerFactory;
            _mapper = mapper;
        }


        public async Task<CreatedFileModel> CreateNotesOfAuthorsFileAsync(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel)
        {
            var fileHandler = _fileHandlerFactory.GetNotesOfAuthorsHandler(saveNoteOfAuthorsModel.Format);

            var createdFileModel = await fileHandler.CreateFileAsync(saveNoteOfAuthorsModel);

            // TODO: MAP TO CREATEDFILEMODEL
            var resultFileModel = _mapper.Map<CreatedFileModel>(createdFileModel);

            resultFileModel.FileName = DateTime.UtcNow.ToString("G") +
                "testname."+resultFileModel.Format.ToString().ToLower();

            // TODO: write to db generated file

            return resultFileModel;
        }
    }
}
