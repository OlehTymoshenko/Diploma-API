using NUnit.Framework;
using BL.Utils;
using DL.Entities.Enums;
using BL.Subdomains.Auth.Services;
using Microsoft.Extensions.Options;
using Common.Configurations.Sections;
using Moq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Auth
{
    [TestFixture]
    public class AuthTests
    {
        Mock<IOptions<AppSettingsSection>> _mockAppSettings;
        TokenService _tokenService;

        [SetUp]
        public void SetUp()
        {
            _mockAppSettings = new Mock<IOptions<AppSettingsSection>>();
            _mockAppSettings.Setup(b => b.Value).Returns(() => new AppSettingsSection() {
                Secret = "Test secret. Test secret. Test secret. Test secret. ",
            });

            _tokenService = new TokenService(_mockAppSettings.Object);
        }


        [Test]
        public void FileFormatToMIME_FileFormatIsDOCX_ReturnCorrectMIME()
        {
            var expectedResult = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            var fileFormat = FileFormat.DOCX;
            var actualMIMEType = FileFormatToMIMETypeConverter.FileFormatToMIME(fileFormat);

            StringAssert.AreEqualIgnoringCase(expectedResult, actualMIMEType);
        }

        [Test]
        public void GenerateRefreshToken_None_ReturnRefreshTokenAsString()
        {
            var actualResultToken = _tokenService.GenerateRefreshToken();

            Assert.IsFalse(string.IsNullOrWhiteSpace(actualResultToken));
        }

        [Test]
        public void GenerateAccessToken_None_ReturnAccessToken()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "test@email.com")
            };

            var actualToken = _tokenService.GenerateAccessToken(claims);

            Assert.IsFalse(string.IsNullOrWhiteSpace(actualToken));
        }


        [Test]
        public void GetClaimsFromAccessToken_None_ReturnTokenClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "test@email.com")
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            var actualClaims = _tokenService.GetClaimsPrincipalFromExpiredToken(accessToken);

            Assert.IsNotNull(actualClaims);
            Assert.IsTrue(actualClaims.Claims.Select(c => new { c.Type, c.Value }).
                Contains(claims.Select(c => new { c.Type, c.Value}).First()));
        }

        [Test]
        public void GetHashFromTextPassword_None_AlwaysReturnTheSameHashForTheSamePassword()
        {
            string testPasswordSalt = "Test_Salt_FoR_P@sword";
            string passwordForTest = "testPassword@31";
            string expectedHash = "+\u007f\u0016O�˗\u0004%PC8���j�ғx\u0010-���9�I���"; // algorithm HMACSHA256

            AuthSecurityHelper authSecurityHelper = new AuthSecurityHelper(testPasswordSalt);


            string actualHash = authSecurityHelper.GetPasswordHash(passwordForTest);


            Assert.AreEqual(expectedHash, actualHash);
        }
    }
}