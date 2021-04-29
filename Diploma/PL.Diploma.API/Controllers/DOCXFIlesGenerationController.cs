using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces.Subdomains.DataForFiles.Services;
using BL.Models.DataForFiles;

namespace PL.Diploma.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    //[Authorize(Policies.Client)]
    [Route("api/[controller]")]
    [ApiController]
    public class DOCXFIlesGenerationController : DiplomaApiControllerBase
    {
        public async Task<ActionResult> GenerateFile()
        {
            return File(fileContents: null, null);
        }
    }
}
