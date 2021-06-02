using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Models.FilesGeneration;
using BL.Subdomains.FilesGeneration.Services;
using Common.Configurations.Sections;
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
    public class DeclensionServiceTests
    {
        Guid TokenForMorpherWebApi => new Guid("8e6d709f-5b2a-4f3d-8a47-003c23f241a0");
        string UrlOfMorpherWebApi => "http://ws3.morpher.ru";

        Mock<IOptions<MorpherSection>> _morpherSection = new Mock<IOptions<MorpherSection>>();

        IDeclensionService _declensionService;

        [SetUp]
        public void SetUp()
        {
            _morpherSection.Setup(o => o.Value).Returns(() => new MorpherSection() {
                MorpherWebApiUrl = UrlOfMorpherWebApi,
                Token = TokenForMorpherWebApi
            });

            _declensionService = new DeclensionService(_morpherSection.Object);
        }


        [Test]
        public void ParseUrk_SurnameInUkr_RightDeclensionFormsOfSurname()
        {
            var surnameForTest = "Дужий";

            var exptectedResult = new InflectedUkrText()
            {
                Nominative = surnameForTest,
                Genitive = "Дужого",
                Accusative = "Дужого",
                Dative = "Дужому",
                Instrumental = "Дужим",
                Prepositional = "Дужим",
                Vocative = "Дужий"
            };


            var actualResult = _declensionService.ParseUkr(surnameForTest);

            Assert.AreEqual(exptectedResult, actualResult);
        }

        [Test]
        public void ParseUrk_Noun_RightDeclensionFormsOfNoun()
        {
            var surnameForTest = "навчальний посібник";

            var exptectedResult = new InflectedUkrText()
            {
                Nominative = surnameForTest,
                Genitive = "навчального посібника",
                Accusative = "навчальний посібник",
                Dative = "навчальному посібнику",
                Instrumental = "навчальним посібником",
                Prepositional = "навчальному посібнику",
                Vocative = "навчальний посібнику"
            };


            var actualResult = _declensionService.ParseUkr(surnameForTest);


            Assert.AreEqual(exptectedResult, actualResult);
        }

    }
}
