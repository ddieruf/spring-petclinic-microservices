using spring_petclinic_visits_api.Domain;
using spring_petclinic_visits_api.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace spring_petclinic_visits_api.Infrastructure.Repository {
  public interface IVisits {
    Task<List<Visit>> FindByPetId(int petId, CancellationToken cancellationToken = default);
    Task<List<Visit>> FindByPetIdIn(int[] petIds, CancellationToken cancellationToken = default);
    Task<Visit> Save(int petId, Visit visit, CancellationToken cancellationToken = default);
  }
}
