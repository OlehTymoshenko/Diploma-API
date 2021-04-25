using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using AutoMapper;
using DL.Entities;
using DL.Interfaces.UnitOfWork;
using BL.Interfaces.Subdomains.DataForFiles.Services;
using BL.Models.DataForFiles;
using Common.Infrastructure.ErrorMessages;
using Common.Infrastructure.Exceptions;

namespace BL.Subdomains.DataForFiles.Services
{
    public class UniversityDepartmentService : IUniversityDepartmentService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public UniversityDepartmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UniversityDepartmentModel>> GetAllAsync()
        {
            var universityDepartments = await _unitOfWork.UniversityDepartments.SelectAsync();

            universityDepartments ??= new List<UniversityDepartment>();

            return _mapper.Map<IEnumerable<UniversityDepartmentModel>>(universityDepartments);
        }

        public async Task<UniversityDepartmentModel> AddAsync(SaveUniversityDepartmentModel saveUniversityDepartmentModel)
        {
            var universityDepartment = _mapper.Map<UniversityDepartment>(saveUniversityDepartmentModel);

            await _unitOfWork.UniversityDepartments.AddAsync(universityDepartment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UniversityDepartmentModel>(universityDepartment);
        }

        public async Task<UniversityDepartmentModel> UpdateAsync(UniversityDepartmentModel universityDepartmentModel)
        {
            var universityDepartment = await _unitOfWork.UniversityDepartments.GetByIdAsync(universityDepartmentModel.Id);

            if (universityDepartment == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWasNotFoundDuringUpdate, HttpStatusCode.NotFound);

            universityDepartment.FullName = universityDepartmentModel.FullName;
            universityDepartment.ShortName = universityDepartmentModel.ShortName;

            _unitOfWork.UniversityDepartments.Update(universityDepartment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UniversityDepartmentModel>(universityDepartment);
        }

        public async Task<UniversityDepartmentModel> DeleteAsync(long id)
        {
            var universityDepartment = await _unitOfWork.UniversityDepartments.GetByIdAsync(id);

            if (universityDepartment == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWithIdWasNotFoundDuringDelete, HttpStatusCode.NotFound);

            _unitOfWork.UniversityDepartments.Delete(universityDepartment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UniversityDepartmentModel>(universityDepartment);
        }
    }
}
