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
    public class PublishingHouseService : IPublishingHouseService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public PublishingHouseService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PublishingHouseModel>> GetAllAsync()
        {
            var publishingHouses = await _unitOfWork.PublishingHouses.SelectAsync();

            publishingHouses ??= new List<PublishingHouse>();

            return _mapper.Map<IEnumerable<PublishingHouseModel>>(publishingHouses);
        }

        public async Task<PublishingHouseModel> AddAsync(SavePublishingHouseModel savePublishingHouseModel)
        {
            var publishingHouse = _mapper.Map<PublishingHouse>(savePublishingHouseModel);

            await _unitOfWork.PublishingHouses.AddAsync(publishingHouse);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PublishingHouseModel>(publishingHouse);
        }

        public async Task<PublishingHouseModel> UpdateAsync(PublishingHouseModel publishingHouseModel)
        {
            var publishingHouse = await _unitOfWork.PublishingHouses.GetByIdAsync(publishingHouseModel.Id);

            if (publishingHouse == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWasNotFoundOnUpdate, HttpStatusCode.NotFound);

            publishingHouse.Name = publishingHouseModel.Name;

            _unitOfWork.PublishingHouses.Update(publishingHouse);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PublishingHouseModel>(publishingHouse);
        }

        public async Task<PublishingHouseModel> DeleteAsync(long id)
        {
            var publishingHouse = await _unitOfWork.PublishingHouses.GetByIdAsync(id);

            if (publishingHouse == null)
                throw new DiplomaApiExpection(DataForFilesErrorMessages.ObjectWithIdWasNotFoundOnDelete, HttpStatusCode.NotFound);

            _unitOfWork.PublishingHouses.Delete(publishingHouse);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PublishingHouseModel>(publishingHouse);
        }

    }
}
