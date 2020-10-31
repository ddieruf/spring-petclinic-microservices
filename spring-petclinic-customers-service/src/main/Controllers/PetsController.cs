using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using spring_petclinic_customers_api.DTOs;
using spring_petclinic_customers_api.Repository;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace spring_petclinic_customers_api.Controllers
{
  [Route("")]
  [ApiController]
  [Produces("application/json")]
  public class PetsController : ControllerBase
  {
    private readonly IPets _petsRepo;
    private readonly ILogger<PetsController> _logger;
    private readonly IOwners _ownersRepo;

    public PetsController(ILogger<PetsController> logger, IPets petsRepo, IOwners ownersRepo)
    {
      _petsRepo = petsRepo;
      _logger = logger;
      _ownersRepo = ownersRepo;
    }

    [HttpGet("petTypes")]
    [ProducesResponseType(typeof(List<DTOs.PetType>), 200)]
    public async Task<ActionResult<List<DTOs.PetType>>> GetPetTypes(CancellationToken cancellationToken)
    {
      var petTypes = await _petsRepo.FindPetTypes(cancellationToken);

      var ret = new List<DTOs.PetType>();
      foreach (var petType in petTypes)
        ret.Add(PetType.ToDTO(petType));

      return Ok(ret);
    }

    [HttpGet("owners/pets/{petId}")]
    [ProducesResponseType(typeof(PetDetails), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PetDetails>> FindPet(int petId, CancellationToken cancellationToken)
    {
      var pet = await _petsRepo.FindById(petId, cancellationToken);

      if (pet == null)
        throw new ResourceNotFoundException("Pet " + petId + " not found");

      return Ok(new PetDetails(pet.Id, pet.Name, pet.Owner.FirstName+" "+pet.Owner.LastName, pet.BirthDate));
    }

    [HttpPost("owners/{ownerId}/pets")]
    [ProducesResponseType(typeof(PetDetails), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResourceNotFoundException), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<PetDetails>> ProcessCreationForm(int ownerId, [FromBody] PetRequest petRequest, CancellationToken cancellationToken)
    {
      var owner = await _ownersRepo.FindById(ownerId, cancellationToken);

      if (owner == null)
        throw new ResourceNotFoundException("Owner " + ownerId + " not found");

      _logger.LogInformation($"Saving pet {petRequest}");
      var newPet = await _petsRepo.Save(ownerId, petRequest, cancellationToken);

      return Created($"owners/pets/{newPet.Id}", new PetDetails(newPet.Id, newPet.Name, newPet.Owner.FirstName + " " + newPet.Owner.LastName, newPet.BirthDate));
    }

    [HttpPut("owners/pets/{petId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> ProcessUpdateForm(int petId, [FromBody] PetRequest petRequest, CancellationToken cancellationToken)
    {
      var pet = await _petsRepo.FindById(petId, cancellationToken);

      if (pet == null)
        throw new ResourceNotFoundException("Pet " + petId + " not found");

      _logger.LogInformation($"Updating pet {petRequest}");
      await _petsRepo.Update(petId, petRequest, cancellationToken);

      return NoContent();
    }
  }
}
