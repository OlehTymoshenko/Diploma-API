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
    public class UniversityDepartmentController : DiplomaApiControllerBase
    {
        readonly IUniversityDepartmentService _universityDepartmentService;

        public UniversityDepartmentController(IUniversityDepartmentService universityDepartmentService)
        {
            _universityDepartmentService = universityDepartmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UniversityDepartmentModel>>> Get()
        {
            var universityDepartmentsModels = await _universityDepartmentService.GetAllAsync();

            return universityDepartmentsModels.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<UniversityDepartmentModel>> Post(SaveUniversityDepartmentModel saveUniversityDepartmentModel)
        {
            return await _universityDepartmentService.AddAsync(saveUniversityDepartmentModel);
        }

        [HttpPut]
        public async Task<ActionResult<UniversityDepartmentModel>> Put(UniversityDepartmentModel universityDepartmentModel)
        {
            return await _universityDepartmentService.UpdateAsync(universityDepartmentModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UniversityDepartmentModel>> Delete(long id)
        {
            return await _universityDepartmentService.DeleteAsync(id);
        }
    }


}
