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
    public class DegreeController : DiplomaApiControllerBase
    {
        readonly IDegreeService _degreeService;

        public DegreeController(IDegreeService degreeService)
        {
            _degreeService = degreeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DegreeModel>>> Get()
        {
            var publishingHouses = await _degreeService.GetAllAsync();

            return publishingHouses.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<DegreeModel>> Post(SaveDegreeModel saveDegreeModel)
        {
            return await _degreeService.AddAsync(saveDegreeModel);
        }

        [HttpPut]
        public async Task<ActionResult<DegreeModel>> Put(DegreeModel degreeModel)
        {
            return await _degreeService.UpdateAsync(degreeModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DegreeModel>> Delete(long id)
        {
            return await _degreeService.DeleteAsync(id);
        }
    }
}
