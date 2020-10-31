using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using spring_petclinic_customers_api.Domain;
using spring_petclinic_customers_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace spring_petclinic_customers_api.Repository
{
  internal class Pets : IPets {
    private readonly ILogger<Pets> _logger;
    private readonly CustomersContext _dbContext;

    public Pets(ILogger<Pets> logger, CustomersContext dbContext) {
      _logger = logger;
      _dbContext = dbContext;
    }

    public Task<List<PetType>> FindPetTypes(CancellationToken cancellationToken = default) {
      return _dbContext.PetTypes.OrderByDescending(q => q.Name).ToListAsync(cancellationToken);
    }

    public Task<PetType> FindPetTypeById(int id, CancellationToken cancellationToken = default) {
      return _dbContext.PetTypes.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<Pet> FindById(int id, CancellationToken cancellationToken = default) {
      return _dbContext.Pets.Include(b => b.Owner).FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<List<Pet>> FindAll(CancellationToken cancellationToken = default) {
      return _dbContext.Pets.Include(b => b.Owner).ToListAsync(cancellationToken);
    }

    public Task<List<Pet>> FindAll(int page, int pageSize, CancellationToken cancellationToken = default) {
      return _dbContext.Pets.Include(b => b.Owner).Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<Pet> Save(int ownerId, DTOs.PetRequest petReuqest, CancellationToken cancellationToken = default) {
      var owner = await _dbContext.Owners.FirstOrDefaultAsync(q => q.Id == ownerId, cancellationToken);

      if (owner == null)
        throw new ArgumentOutOfRangeException(nameof(ownerId));

      var newPet = new Pet(petReuqest.Name, petReuqest.BirthDate, petReuqest.PetTypeId, ownerId);

      _dbContext.Pets.Add(newPet);
      await _dbContext.SaveChangesAsync(cancellationToken);
      return newPet;
    }
    public async Task<Pet> Update(int petId, DTOs.PetRequest petReuqest, CancellationToken cancellationToken = default) {
      var pet = await FindById(petId);

      pet.SetBirthDate(petReuqest.BirthDate);
      pet.SetName(petReuqest.Name);

      _dbContext.Pets.Update(pet);
      await _dbContext.SaveChangesAsync(cancellationToken);
      return pet;
    }
    //public Task Delete(Pet pet, CancellationToken cancellationToken = default)
    //{
    //  _dbContext.Pets.Remove(pet);
    //  return _dbContext.SaveChangesAsync(cancellationToken);
    //}
  }
}
