using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Subdomains.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers;
using BL.Subdomains.FilesGeneration.Services;
using Common.Configurations.Sections;
using DL.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.FilesGeneration
{
    [TestFixture]
    public class FileHandlerFactoryTests
    {
        Mock<ObjectWrapperForMoq<IServiceProvider>> _serviceProvider = new Mock<ObjectWrapperForMoq<IServiceProvider>>();
        FileHandlerFactory _fileHandlerFactory; 

        [SetUp]
        public void SetUp()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<INotesOfAuthorsHandler, NotesOfAuthorsInDocxHandler>();
            serviceCollection.AddScoped<IProtocolOfMeetingOfExpertCommissionHandler, ProtocolOfMeetingOfExpertCommissionInDocxHandler>();
            serviceCollection.AddScoped<IExpertiseActHandler, ExpertiseActInDocxHandler>();
            serviceCollection.AddScoped<IDeclensionService, DeclensionService>();
            serviceCollection.Configure<MorpherSection>(s => { });

            _serviceProvider.SetupGet(x => x.Value).Returns(serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider);

            _fileHandlerFactory = new FileHandlerFactory(_serviceProvider.Object.Value);
        }

        [Test]
        [TestCase(FileFormat.DOCX)]
        public void GetNotesOfAuthorsHandler_FormatOfHandler_NotNullReferenceOfHandler(FileFormat fileFormat)
        {
            var actualResult = _fileHandlerFactory.GetNotesOfAuthorsHandler(fileFormat);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        [TestCase(FileFormat.DOCX)]
        public void GetExpertiseActHandle_FormatOfHandler_NotNullReferenceOfHandler(FileFormat fileFormat)
        {
            var actualResult = _fileHandlerFactory.GetExpertiseActHandler(fileFormat);

            Assert.IsNotNull(actualResult);
        }

        [Test]
        [TestCase(FileFormat.DOCX)]
        public void GetProtocolOfMeetingOfExpertCommissionHandler_FormatOfHandler_NotNullReferenceOfHandler(FileFormat fileFormat)
        {
            var actualResult = _fileHandlerFactory.GetProtocolOfMeetingOfExpertCommissionHandler(fileFormat);

            Assert.IsNotNull(actualResult);
        }
    }

    public class ObjectWrapperForMoq<T> where T : class
    {
        public virtual T Value { get; set; }
    }
}
