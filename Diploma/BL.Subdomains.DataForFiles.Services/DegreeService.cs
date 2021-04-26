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
    public class DegreeService : IDegreeService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public DegreeService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DegreeModel>> GetAllAsync()
        {
            var degrees = await _unitOfWork.Degrees.SelectAsync();

            degrees ??= new List<Degree>();

            return _mapper.Map<IEnumerable<DegreeModel>>(degrees);
        }

        public async Task<DegreeModel> AddAsync(SaveDegreeModel saveDegreeModel)
        {
            var degree = _mapper.Map<Degree>(saveDegreeModel);

            await _unitOfWork.Degrees.AddAsync(degree);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DegreeModel>(degree);
        }

        public async Task<DegreeModel> UpdateAsync(DegreeModel degreeModel)
        {
            var degree = await _unitOfWork.Degrees.GetByIdAsync(degreeModel.Id);

            if (degree == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWasNotFoundOnUpdate, HttpStatusCode.NotFound);

            degree.Name = degreeModel.Name;

            _unitOfWork.Degrees.Update(degree);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DegreeModel>(degree);
        }

        public async Task<DegreeModel> DeleteAsync(long id)
        {
            var degree = await _unitOfWork.Degrees.GetByIdAsync(id);

            if (degree == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWithIdWasNotFoundOnDelete, HttpStatusCode.NotFound);

            _unitOfWork.Degrees.Delete(degree);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DegreeModel>(degree);
        }

    }


}
