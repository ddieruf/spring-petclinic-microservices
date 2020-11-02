using System;

namespace spring_petclinic_visits_api.Domain {
  public partial class Visit {
    public Visit(int petId, DateTime? visitDate, string description, int id = default) {
      Id = id;
      PetId = petId;
      VisitDate = visitDate;
      Description = description;
    }

    public int Id { get; private set; }
    public int PetId { get; private set; }
    public DateTime? VisitDate { get; private set; }
    public string Description { get; private set; }

    public void SetPetId(int petId){
      PetId = petId;
    }
  }
}
