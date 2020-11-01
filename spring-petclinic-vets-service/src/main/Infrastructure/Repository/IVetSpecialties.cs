using spring_petclinic_vets_api.Domain;
using System.Collections.Generic;

namespace spring_petclinic_vets_api.Infrastructure.Repository {
  public interface IVetSpecialties {
    IEnumerable<VetSpecialty> FindAllBySpecialtyId(int specialtyId);
    IEnumerable<VetSpecialty> FindAllByVetId(int vetId);
  }
}
