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
using System.Linq;

namespace BL.Subdomains.DataForFiles.Services
{
    public class ScientistService : IScientistService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public ScientistService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScientistModel>> GetAllAsync()
        {
            var scientists = await _unitOfWork.Scientists.SelectAsync(s => s.Degrees);

            scientists ??= new List<Scientist>();

            return _mapper.Map<IEnumerable<ScientistModel>>(scientists);
        }

        public async Task<ScientistModel> AddAsync(SaveScientistModel saveScientistModel)
        {
            var scientist = _mapper.Map<Scientist>(saveScientistModel);

            foreach(var degreeId in saveScientistModel.DegreesIds)
            {
                var degree = await _unitOfWork.Degrees.GetByIdAsync(degreeId);
                scientist.Degrees.Add(degree);
            }

            await _unitOfWork.Scientists.AddAsync(scientist);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScientistModel>(scientist);
        }

        public async Task<ScientistModel> UpdateAsync(UpdateScientistModel updateScientistModel)
        {
            var scientist = (await _unitOfWork.Scientists.SelectAsync(s => s.Id == updateScientistModel.Id, s => s.Degrees))
                .FirstOrDefault();

            if (scientist == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWasNotFoundOnUpdate, HttpStatusCode.NotFound);

            scientist.FirstName = updateScientistModel.FirstName;
            scientist.LastName = updateScientistModel.LastName;
            scientist.MiddleName = updateScientistModel.MiddleName;
            
            scientist.Degrees = new List<Degree>();

            foreach (var degreeId in updateScientistModel.DegreesIds)
            {
                var degree = await _unitOfWork.Degrees.GetByIdAsync(degreeId);
                
                if(degree != null) 
                    scientist.Degrees.Add(degree);
            }

            _unitOfWork.Scientists.Update(scientist);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScientistModel>(scientist);
        }

        public async Task<ScientistModel> DeleteAsync(long id)
        {
            var scientist = await _unitOfWork.Scientists.GetByIdAsync(id);

            if (scientist == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWithIdWasNotFoundOnDelete, HttpStatusCode.NotFound);

            _unitOfWork.Scientists.Delete(scientist);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ScientistModel>(scientist);
        }
    }
}
