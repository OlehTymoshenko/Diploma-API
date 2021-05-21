using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using Common.Infrastructure.Exceptions;
using DL.Entities;
using DL.Entities.Enums;
using DL.Interfaces.UnitOfWork;

namespace BL.Subdomains.FilesGeneration
{
    public class FilesGenerationService : IFilesGenerationService
    {
        readonly IFileHandlerFactory _fileHandlerFactory;
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public FilesGenerationService(IFileHandlerFactory fileHandlerFactory, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _fileHandlerFactory = fileHandlerFactory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<CreatedFileModel> CreateNotesOfAuthorsAsync(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel, IEnumerable<Claim> userClaims)
        {
            var fileHandler = _fileHandlerFactory.GetNotesOfAuthorsHandler(saveNoteOfAuthorsModel.Format);
            var createdFileModel = await fileHandler.CreateFileAsync(saveNoteOfAuthorsModel);

            var user = await GetUserFromDbAsync(userClaims);

            var resultFileModel = _mapper.Map<CreatedFileModel>(createdFileModel);
            resultFileModel.FileName = GetFileName(resultFileModel.Type, resultFileModel.Format, user.FirstName, user.LastName);

            await _unitOfWork.GeneratedFiles.AddAsync(new GeneratedFile()
            {
                Format = resultFileModel.Format,
                Type = resultFileModel.Type,
                Name = resultFileModel.FileName,
                CreationDate = DateTime.UtcNow,
                User = user
            });

            await _unitOfWork.SaveChangesAsync();

            return resultFileModel;
        }

        public async Task<CreatedFileModel> CreateExpertiseActAsync(SaveExpertiseActModel saveExpertiseActModel, IEnumerable<Claim> userClaims)
        {
            var fileHandler = _fileHandlerFactory.GetExpertiseActHandler(saveExpertiseActModel.Format);
            var createdFileModel = await fileHandler.CreateFileAsync(saveExpertiseActModel);

            var user = await GetUserFromDbAsync(userClaims);

            var resultFileModel = _mapper.Map<CreatedFileModel>(createdFileModel);
            resultFileModel.FileName = GetFileName(resultFileModel.Type, resultFileModel.Format, user.FirstName, user.LastName);

            await _unitOfWork.GeneratedFiles.AddAsync(new GeneratedFile()
            {
                Format = resultFileModel.Format,
                Type = resultFileModel.Type,
                Name = resultFileModel.FileName,
                CreationDate = DateTime.UtcNow,
                User = user
            });

            await _unitOfWork.SaveChangesAsync();

            return resultFileModel;
        }

        public async Task<CreatedFileModel> CreateProtocolOfMeetingOfExpertCommissionAsync(SaveProtocolOfMeetingOfExpertCommissionModel saveProtocolOfMeetingOfExpertCommissionModel, 
            IEnumerable<Claim> userClaims)
        {
            var fileHandler = _fileHandlerFactory.GetProtocolOfMeetingOfExpertCommissionHandler(saveProtocolOfMeetingOfExpertCommissionModel.Format);
            var createdFileModel = await fileHandler.CreateFileAsync(saveProtocolOfMeetingOfExpertCommissionModel);

            var user = await GetUserFromDbAsync(userClaims);

            var resultFileModel = _mapper.Map<CreatedFileModel>(createdFileModel);
            resultFileModel.FileName = GetFileName(resultFileModel.Type, resultFileModel.Format, user.FirstName, user.LastName);

            await _unitOfWork.GeneratedFiles.AddAsync(new GeneratedFile()
            {
                Format = resultFileModel.Format,
                Type = resultFileModel.Type,
                Name = resultFileModel.FileName,
                CreationDate = DateTime.UtcNow,
                User = user
            });

            await _unitOfWork.SaveChangesAsync();

            return resultFileModel;
        }


        private Task<User> GetUserFromDbAsync(IEnumerable<Claim> userClaims)
        {
            var userEmail = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
               throw new DiplomaApiExpection("Provided claims doesn't contain user email", System.Net.HttpStatusCode.BadRequest);

            return _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        }

        private string GetFileName(FileType fileType, FileFormat fileFormat, string userFirstName, string userLastName)
        {
            return $"{fileType}_{userFirstName}_{userLastName}_{DateTime.UtcNow.ToString("g").Replace(' ', '_')}" +
                $".{fileFormat.ToString().ToLower()}";
        }
    }
}
