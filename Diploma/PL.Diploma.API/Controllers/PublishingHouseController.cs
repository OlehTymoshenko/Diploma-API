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
    public class PublishingHouseController : DiplomaApiControllerBase
    {
        readonly IPublishingHouseService _publishingHouseService;

        public PublishingHouseController(IPublishingHouseService publishingHouseService)
        {
            _publishingHouseService = publishingHouseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublishingHouseModel>>> Get()
        {
            var publishingHouses = await _publishingHouseService.GetAllAsync();
            
            return publishingHouses.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<PublishingHouseModel>> Post(SavePublishingHouseModel savePublishingHouseModel)
        {
             return await _publishingHouseService.AddAsync(savePublishingHouseModel);
        }

        [HttpPut]
        public async Task<ActionResult<PublishingHouseModel>> Put(PublishingHouseModel publishingHouseModel)
        {
            return await _publishingHouseService.UpdateAsync(publishingHouseModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PublishingHouseModel>> Delete(long id)
        {
             return await _publishingHouseService.DeleteAsync(id);
        }
    }
}
