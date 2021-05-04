using AutoMapper;
using BL.Models.FilesGeneration;
using BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
