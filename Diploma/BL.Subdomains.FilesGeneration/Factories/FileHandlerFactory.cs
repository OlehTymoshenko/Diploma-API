using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using BL.Interfaces.Subdomains.FilesGeneration;
using DL.Entities.Enums;

namespace BL.Subdomains.FilesGeneration
{
    public class FileHandlerFactory : IFileHandlerFactory
    {
        readonly IServiceProvider _serviceProvider;

        public FileHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public INotesOfAuthorsHandler GetNotesOfAuthorsHandler(FileFormat fileFormat)
        {
            var notesOfAuthorsHandlers = _serviceProvider.GetServices<INotesOfAuthorsHandler>();

            var handler = notesOfAuthorsHandlers.FirstOrDefault(h => h.Format == fileFormat);

            return handler ?? throw new ArgumentException($"{FileType.NoteOfAuthors} file type isn't supported in " +
                                                          $"{fileFormat} format");
        }

        public IExpertiseActHandler GetExpertiseActHandler(FileFormat fileFormat)
        {
            var handlersForEachFormat = _serviceProvider.GetServices<IExpertiseActHandler>();

            var handler = handlersForEachFormat.FirstOrDefault(h => h.Format == fileFormat);

            return handler ?? throw new ArgumentException($"{FileType.ExpertiseAct} file type isn't supported in " +
                                                          $"{fileFormat} format");
        }

        public IProtocolOfMeetingOfExpertCommissionHandler GetProtocolOfMeetingOfExpertCommissionHandler(FileFormat fileFormat)
        {
            var handlersForEachFormat = _serviceProvider.GetServices<IProtocolOfMeetingOfExpertCommissionHandler>();

            var handler = handlersForEachFormat.FirstOrDefault(h => h.Format == fileFormat);

            return handler ?? throw new ArgumentException($"{FileType.ProtocolOfMeetingOfExpertCommission} file type isn't supported in " +
                                                          $"{fileFormat} format");
        }
    }
}
