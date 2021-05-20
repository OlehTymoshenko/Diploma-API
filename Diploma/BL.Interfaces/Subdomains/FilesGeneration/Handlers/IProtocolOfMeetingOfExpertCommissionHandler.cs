using BL.Models.FilesGeneration;
using System.Threading.Tasks;

namespace BL.Interfaces.Subdomains.FilesGeneration
{
    public interface IProtocolOfMeetingOfExpertCommissionHandler : IFileHandler
    {
        Task<FileModel> CreateFileAsync(SaveProtocolOfMeetingOfExpertCommissionModel saveProtocolOfMeetingOfExpertCommissionModel);
    }
}
