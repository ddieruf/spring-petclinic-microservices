using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using spring_petclinic_visits_api.Repository;

namespace spring_petclinic_visits_api.Controllers
{
  [Route("")]
  [ApiController]
  [Produces("application/json")]
  public class VisitsController : ControllerBase
  {
    private readonly ILogger<VisitsController> _logger;
    private readonly IVisits _visitsRepo;

    public VisitsController(ILogger<VisitsController> logger, IVisits visitsRepo)
    {
      _logger = logger;
      _visitsRepo = visitsRepo;
    }

    [HttpGet("owners/{a}/pets/{petId:int}/visits")]
    [ProducesResponseType(typeof(List<DTOs.Visit>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<DTOs.Visit>>> Visits(int petId, CancellationToken cancellationToken)
    {
      var visits = await _visitsRepo.FindByPetId(petId, cancellationToken);
      return Ok(visits);
    }

    [HttpGet("pets/visits")]
    [ProducesResponseType(typeof(List<DTOs.Visit>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<DTOs.Visit>>> VisitsMultiGet([FromQuery] int[] petId, CancellationToken cancellationToken) {
      var visits = await _visitsRepo.FindByPetIdIn(petId, cancellationToken);
      return Ok(visits);
    }

    [HttpPost("owners/{a}/pets/{petId:int}/visits")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create(int petId,[FromBody] DTOs.Visit visit, CancellationToken cancellationToken) {
      visit.PetId = petId;
      _logger.LogInformation($"Saving visit {visit}");
      var newVisit = await _visitsRepo.Save(petId, visit, cancellationToken);
      return Created($"owners/pets/{petId}/visits", newVisit);
    }
  }
}
