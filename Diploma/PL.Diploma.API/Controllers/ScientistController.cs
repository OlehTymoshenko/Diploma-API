using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces.Subdomains.DataForFiles.Services;
using BL.Models.DataForFiles;
using Microsoft.AspNetCore.Authorization;
using PL.Utils.Auth;

namespace PL.Diploma.API.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Policies.Client)]
    [Route("api/[controller]")]
    [ApiController]
    public class ScientistController : DiplomaApiControllerBase
    {
        readonly IScientistService _scientistService;

        public ScientistController(IScientistService scientistService)
        {
            _scientistService = scientistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScientistModel>>> Get()
        {
            var universityDepartmentsModels = await _scientistService.GetAllAsync();

            return universityDepartmentsModels.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<ScientistModel>> Post(SaveScientistModel saveScientistModel)
        {
            return await _scientistService.AddAsync(saveScientistModel);
        }

        [HttpPut]
        public async Task<ActionResult<ScientistModel>> Put(UpdateScientistModel updateScientistModel)
        {
            return await _scientistService.UpdateAsync(updateScientistModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ScientistModel>> Delete(long id)
        {
            return await _scientistService.DeleteAsync(id);
        }
    }
}
