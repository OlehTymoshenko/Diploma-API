using BL.Interfaces.Subdomains.FilesGeneration.Services;
using BL.Models.FilesGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Utils.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Diploma.API.Controllers
{

    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Policies.Client)]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratedFilesController : DiplomaApiControllerBase
    {
        IGeneratedFilesService _generatedFilesService;

        public GeneratedFilesController(IGeneratedFilesService generatedFilesService)
        {
            _generatedFilesService = generatedFilesService;
        }

        [HttpGet("get-list-of-generated-files")]
        public async Task<ActionResult<IEnumerable<DescriptionOfGeneratedFile>>> GetListOfGeneratedFilesForCurrentUser()
        {
            var generatedFiles = await _generatedFilesService.GetUserGeneratedFilesAsync(HttpContext.User.Claims);

            return generatedFiles.ToList();
        }
    }
}
