using AutoMapper;
using DL.Entities;
using BL.Models.DataForFiles;

namespace BL.Infrastructure.AutoMapperProfiles
{
    public class DataForFilesProfile : Profile
    {
        public DataForFilesProfile()
        {
            #region Model to entity

            CreateMap<PublishingHouseModel, PublishingHouse>();
            CreateMap<SavePublishingHouseModel, PublishingHouse>();

            CreateMap<UniversityDepartmentModel, UniversityDepartment>();
            CreateMap<SaveUniversityDepartmentModel, UniversityDepartment>();

            CreateMap<DegreeModel, Degree>();
            CreateMap<SaveDegreeModel, Degree>();

            CreateMap<ScientistModel, Scientist>();
            CreateMap<SaveScientistModel, Scientist>().
                ForMember(d => d.Degrees, opt => opt.Ignore());

            #endregion

            #region Entity to model

            CreateMap<PublishingHouse, PublishingHouseModel>();

            CreateMap<UniversityDepartment, UniversityDepartmentModel>();

            CreateMap<Degree, DegreeModel>();

            CreateMap<Scientist, ScientistModel>();

            #endregion
        }
    }
}
