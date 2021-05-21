using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DL.Entities;
using DL.Interfaces.UnitOfWork;
using BL.Models.FilesGeneration;
using BL.Interfaces.Subdomains.FilesGeneration.Services;
using Common.Infrastructure.Exceptions;

namespace BL.Subdomains.FilesGeneration.Services
{
    public class GeneratedFileService : IGeneratedFilesService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public GeneratedFileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DescriptionOfGeneratedFile>> GetUserGeneratedFiles(IEnumerable<Claim> userClaims)
        {
            var user =  await GetUserFromDbAsync(userClaims);

            var generatedFiles = await _unitOfWork.GeneratedFiles.SelectAsync(g => g.UserId == user.Id);

            return _mapper.Map<IEnumerable<DescriptionOfGeneratedFile>>(generatedFiles);
        }


        private Task<User> GetUserFromDbAsync(IEnumerable<Claim> userClaims)
        {
            var userEmail = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
               throw new DiplomaApiExpection("Provided claims doesn't contain user email", System.Net.HttpStatusCode.BadRequest);

            return _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        }
    }
}
