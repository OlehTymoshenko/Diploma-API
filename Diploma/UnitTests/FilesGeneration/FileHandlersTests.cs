using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.FilesHandlers;
using BL.Subdomains.FilesGeneration.Services;
using Common.Configurations.Sections;
using Common.Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTests.FilesGeneration
{
    [TestFixture]
    public class FileHandlersTests
    {
        Guid TokenForMorpherWebApi => new Guid("8e6d709f-5b2a-4f3d-8a47-003c23f241a0");
        string UrlOfMorpherWebApi => "http://ws3.morpher.ru";

        Mock<IOptions<MorpherSection>> _morpherSection = new Mock<IOptions<MorpherSection>>();

        INotesOfAuthorsHandler _notesOfAuthorsHandler;
        IExpertiseActHandler _expertiseActHandler;
        IProtocolOfMeetingOfExpertCommissionHandler _protocolOfMeetingHandler;
        

        [SetUp]
        public void SetUp()
        {
            _morpherSection.Setup(o => o.Value).Returns(() => new MorpherSection()
            {
                MorpherWebApiUrl = UrlOfMorpherWebApi,
                Token = TokenForMorpherWebApi
            });

            var declensionService = new DeclensionService(_morpherSection.Object);

            _notesOfAuthorsHandler = new NotesOfAuthorsInDocxHandler(declensionService);
            _expertiseActHandler = new ExpertiseActInDocxHandler(declensionService);
            _protocolOfMeetingHandler = new ProtocolOfMeetingOfExpertCommissionInDocxHandler(declensionService);
        }


        [Test]
        public async Task CreateNotesOfAuthors_ValidDataForFileCreation_CreatedFileModel()
        {
            var expectedResult = new FileModel()
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                Type = DL.Entities.Enums.FileType.NoteOfAuthors
            };

            var dataForFileCreation = new SaveNoteOfAuthorsModel() 
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                Authors = new System.Collections.Generic.List<Scientist>() { 
                    new Scientist()
                    {
                        FullName = "Тимошенко Олег Олексійович",
                        Degrees = new System.Collections.Generic.List<string>()
                        {
                            "студент групи 545Б"
                        }
                    }
                },
                PublishingDate = DateTime.UtcNow,
                PublishingNameWithItsStatics = "бакалаврська робота...",
                UniversityDepartmentName = "Кафедра 503",
                PublishingHouse = "Видавництво ХАІ",
                FullNameOfChiefOfUniversityDepartment = "Харченко"
            };


            var actualResult = await _notesOfAuthorsHandler.CreateFileAsync(dataForFileCreation);


            Assert.AreEqual(expectedResult.Format, actualResult.Format);
            Assert.AreEqual(expectedResult.Type, actualResult.Type);

            Assert.IsNotNull(actualResult.FileAsBytes);
            Assert.IsNotNull(actualResult.FileAsBytes.Length > 1000); // correct created file size is always greater than 1000 bytes 
        }

        [Test]
        public void CreateNotesOfAuthors_InValidDataForFileCreation_ThrowDiplomaApiException()
        {
            var expectedResult = new FileModel();

            var dataForFileCreation = new SaveNoteOfAuthorsModel();


            Assert.ThrowsAsync<DiplomaApiExpection>(async () => 
                await _notesOfAuthorsHandler.CreateFileAsync(dataForFileCreation));
        }

        [Test]
        public async Task CreateExpertiseAct_ValidDataForFileCreation_CreatedFileModel()
        {
            var expectedResult = new FileModel()
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                Type = DL.Entities.Enums.FileType.ExpertiseAct
            };

            var dataForFileCreation = new SaveExpertiseActModel()
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                FacultyNumber = 1,
                AuthorsOfThePublication = new Scientist[] {
                    new Scientist()
                    {
                        FullName = "Тимошенко Олег Олексійович",
                        Degrees = new System.Collections.Generic.List<string>()
                        {
                            "студент групи 545Б"
                        }
                    }
                },
                ActCreationDate = DateTime.UtcNow,
                PublishingNameWithItsStatics = "бакалаврська робота...",
                HeadOfTheCommission = new Scientist()
                {
                    FullName = "Тестове Повне ім'я", 
                    Degrees = new System.Collections.Generic.List<string>()
                    {
                        "тестове наукове звання"
                    }
                },
                ChiefOfSecurityDepartment = "Тестове Повне ім'я",
                ProvostName = "Тестове Повне ім'я",
                MembersOfTheCommission = new Scientist[]
                {
                    new Scientist()
                    {
                        FullName = "Тестове Повне ім'я",
                        Degrees = new System.Collections.Generic.List<string>()
                        {
                            "тестове наукове звання"
                        }
                    }
                },
                SecretaryOfTheCommission = "Тестове Повне ім'я"
            };


            var actualResult = await _expertiseActHandler.CreateFileAsync(dataForFileCreation);


            Assert.AreEqual(expectedResult.Format, actualResult.Format);
            Assert.AreEqual(expectedResult.Type, actualResult.Type);

            Assert.IsNotNull(actualResult.FileAsBytes);
            Assert.IsNotNull(actualResult.FileAsBytes.Length > 1000); // correct created file size is always greater than 1000 bytes 
        }

        [Test]
        public void CreateExpertiseAct_InValidDataForFileCreation_ThrowDiplomaApiException()
        {
            var expectedResult = new FileModel();

            var dataForFileCreation = new SaveExpertiseActModel();


            Assert.ThrowsAsync<DiplomaApiExpection>(async () =>
                await _expertiseActHandler.CreateFileAsync(dataForFileCreation));
        }

        [Test]
        public async Task CreateProtocolOfMeeting_ValidDataForFileCreation_CreatedFileModel()
        {
            var expectedResult = new FileModel()
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                Type = DL.Entities.Enums.FileType.ProtocolOfMeetingOfExpertCommission
            };

            var dataForFileCreation = new SaveProtocolOfMeetingOfExpertCommissionModel()
            {
                Format = DL.Entities.Enums.FileFormat.DOCX,
                FacultyNumber = 1,
                ActCopyNumber = 1,
                ProtocolCreationDate = DateTime.UtcNow,
                PublishingNameWithItsStatics = "бакалаврська робота...",
                DoesCommissionAllowAIssuingOfThePublication = true,
                DoesPubliscationContainServiceInformation = false,
                HeadOfTheCommissionName = "Тестове Повне ім'я",
                IsPublicationAStateSecret = false,
                MembersOfTheCommissionNames = new string[]
                {
                    "Тестове Повне ім'я", "Тестове Повне ім'я2"
                },
                SpeakersOfTheCommissionName = new string[]
                {
                    "Тестове Повне ім'я"
                },
                SecretaryOfTheCommissionName = "Тестове Повне ім'я",
                ChiefOfSecurityDepartment = "Тестове Повне ім'я"
            };


            var actualResult = await _protocolOfMeetingHandler.CreateFileAsync(dataForFileCreation);


            Assert.AreEqual(expectedResult.Format, actualResult.Format);
            Assert.AreEqual(expectedResult.Type, actualResult.Type);

            Assert.IsNotNull(actualResult.FileAsBytes);
            Assert.IsNotNull(actualResult.FileAsBytes.Length > 1000); // correct created file size is always greater than 1000 bytes 
        }

        [Test]
        public void CreateProtocolOfMeeting_InValidDataForFileCreation_ThrowDiplomaApiException()
        {
            var expectedResult = new FileModel();

            var dataForFileCreation = new SaveProtocolOfMeetingOfExpertCommissionModel();


            Assert.ThrowsAsync<DiplomaApiExpection>(async () =>
                await _protocolOfMeetingHandler.CreateFileAsync(dataForFileCreation));
        }
    }
}
