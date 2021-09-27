using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using PL.Utils.Auth;

namespace PL.Diploma.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Policies.Client)]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesGenerationController : DiplomaApiControllerBase
    {
        readonly IFilesGenerationService _filesGenerationService;

        public FilesGenerationController(IFilesGenerationService filesGenerationService)
        {
            _filesGenerationService = filesGenerationService;
        }

        [HttpPost("generate-notes-of-authors")]
        public async Task<ActionResult> GenerateNotesOfAuthors(SaveNoteOfAuthorsModel saveNoteOfAuthorsModel)
        {
            var createdFile = await _filesGenerationService.CreateNotesOfAuthorsAsync(saveNoteOfAuthorsModel, 
                HttpContext.User.Claims);

            return File(createdFile.FileAsBytes, createdFile.MIMEType, createdFile.FileName);
        }

        [HttpPost("generate-protocol-of-meeting-of-expert-commission")]
        public async Task<ActionResult> GenerateProtocolOfMeetingOfExpertCommission(SaveProtocolOfMeetingOfExpertCommissionModel saveProtocolOfMeetingOfExpertCommissionModel)
        {
            var createdFile = await _filesGenerationService.CreateProtocolOfMeetingOfExpertCommissionAsync(saveProtocolOfMeetingOfExpertCommissionModel,
                HttpContext.User.Claims);

            return File(createdFile.FileAsBytes, createdFile.MIMEType, createdFile.FileName);
        }

        [HttpPost("generate-expertise-act")]
        public async Task<ActionResult> GenerateExpertiseAct(SaveExpertiseActModel saveExpertiseActModel)
        {
            var createdFile = await _filesGenerationService.CreateExpertiseActAsync(saveExpertiseActModel, HttpContext.User.Claims);

            return File(createdFile.FileAsBytes, createdFile.MIMEType, createdFile.FileName);
        }

    }
}
