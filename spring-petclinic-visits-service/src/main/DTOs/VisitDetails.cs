using spring_petclinic_visits_api.Domain;
using System;
using System.Text.Json.Serialization;

namespace spring_petclinic_visits_api.DTOs {
  public class VisitDetails {
    public VisitDetails() { }

    public VisitDetails(int id, int petId, DateTime? visitDate, string description) {
      Id = id;
      PetId = petId;
      VisitDate = visitDate;
      Description = description;
    }

    public int Id { get; set; }
    public int PetId { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? VisitDate { get; set; }
    public string Description { get; set; }

    public static VisitDetails FromVisit(Visit visit) {
      return new VisitDetails() {
        Id = visit.Id,
        PetId = visit.PetId,
        VisitDate = visit.VisitDate,
        Description = visit.Description
      };
    }
  }
}
