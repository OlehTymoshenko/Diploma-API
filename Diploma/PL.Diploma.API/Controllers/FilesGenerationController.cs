using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Models.FilesGeneration;
using Microsoft.AspNetCore.Authorization;
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
            var createdFile = await _filesGenerationService.CreateNotesOfAuthorsFileAsync(saveNoteOfAuthorsModel, 
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

        /*[HttpPost("generate-expert-commission-act")]
        public async Task<ActionResult> GenerateProtocolOfMeetingOfExpertCommission(SaveExpertCommissionActModel saveExpertCommissionAct)
        {
            var createdFile = await _filesGenerationService.CreateExpertCommissionActAsync(saveExpertCommissionAct,
                HttpContext.User.Claims);

            return File(createdFile.FileAsBytes, createdFile.MIMEType, createdFile.FileName);
        }*/

    }
}
