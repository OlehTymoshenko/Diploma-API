using AutoMapper;
using BL.Models.FilesGeneration;
using BL.Utils;
using DL.Entities;

namespace BL.Infrastructure.AutoMapperProfiles
{
    public class FilesGenerationProfile : Profile
    {


        public FilesGenerationProfile()
        {
            CreateMap<FileModel, CreatedFileModel>().
                ForMember(c => c.FileName, opt => opt.Ignore()).
                ForMember(c => c.MIMEType, opt => opt.MapFrom(f => 
                    FileFormatToMIMETypeConverter.FileFormatToMIME(f.Format))
                );

            CreateMap<GeneratedFile, DescriptionOfGeneratedFile>();
        }
    }
}
