using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using spring_petclinic_visits_api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace spring_petclinic_visits_api.Infrastructure.Repository {
  internal class Visits : IVisits {
    private readonly ILogger<Visits> _logger;
    private readonly VisitsContext _dbContext;

    public Visits(ILogger<Visits> logger, VisitsContext dbContext) {
      _logger = logger;
      _dbContext = dbContext;
    }

    public Task<List<Visit>> FindByPetId(int petId, CancellationToken cancellationToken = default) {
      return _dbContext.Visits.Where(q => q.PetId == petId).ToListAsync(cancellationToken);
    }
    public Task<List<Visit>> FindByPetIdIn(List<int> petIds, CancellationToken cancellationToken = default) {
      return _dbContext.Visits.Where(q => petIds.Any(r => r == q.PetId)).ToListAsync(cancellationToken);
    }
    public async Task<Visit> Save(int petId, Visit visit, CancellationToken cancellationToken = default) {
      visit.SetPetId(petId);

      _dbContext.Visits.Add(visit);
      await _dbContext.SaveChangesAsync(cancellationToken);
      return visit;
    }
  }
}
