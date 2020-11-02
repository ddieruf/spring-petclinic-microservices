using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using spring_petclinic_visits_api.Domain;
using spring_petclinic_visits_api.DTOs;
using spring_petclinic_visits_api.Infrastructure.Repository;

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
    [HttpGet("owners/pets/{petId:int}/visits")]
    [ProducesResponseType(typeof(List<VisitDetails>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<VisitDetails>>> Visits(int petId, CancellationToken cancellationToken)
    {
      var visits = await _visitsRepo.FindByPetId(petId, cancellationToken);

      var ret = new List<VisitDetails>();
      foreach (var visit in visits)
        ret.Add(VisitDetails.FromVisit(visit));

      return Ok(ret);
    }

    [HttpGet("pets/visits")]
    [ProducesResponseType(typeof(List<VisitDetails>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<VisitDetails>>> VisitsMultiGet([FromQuery] int[] petId, CancellationToken cancellationToken) {
      var visits = await _visitsRepo.FindByPetIdIn(petId, cancellationToken);

      var ret = new List<VisitDetails>();
      foreach (var visit in visits)
        ret.Add(VisitDetails.FromVisit(visit));

      return Ok(ret);
    }

    [HttpPost("owners/{a}/pets/{petId:int}/visits")]
    [HttpPost("owners/pets/{petId:int}/visits")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> Create(int petId,[FromBody] VisitRequest visitRequest, CancellationToken cancellationToken) {
      _logger.LogInformation($"Saving visit {visitRequest}");

      var visit = new Visit(petId, visitRequest.VisitDate, visitRequest.Description);
      var newVisit = await _visitsRepo.Save(petId, visit, cancellationToken);
      return Created($"owners/pets/{petId}/visits", VisitDetails.FromVisit(newVisit));
    }
  }
}
