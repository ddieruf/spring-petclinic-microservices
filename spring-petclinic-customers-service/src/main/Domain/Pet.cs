
using System;
using System.Text.Json.Serialization;

namespace spring_petclinic_customers_api.Domain {
  public partial class Pet
  {
    public Pet(string name, DateTime? birthDate, int typeId, int ownerId, int id = default){
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      BirthDate = birthDate;
      TypeId = typeId;
      OwnerId = ownerId;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public int TypeId { get; private set; }
    public int OwnerId { get; private set; }

    [JsonIgnore]
    public virtual Owner Owner { get; set; }
    [JsonIgnore]
    public virtual PetType PetType { get; set; }
    
    public override string ToString()
    {
      return $@"id:{this.Id},
  name: {this.Name},
  birthDate: {this.BirthDate},
  type: {this.PetType},
  ownerFirstname: {this.Owner?.FirstName},
  ownerLastname: {this.Owner?.LastName}";
    }

    public void SetBirthDate(DateTime? birthDate) {
      BirthDate = birthDate;
    }
    public void SetName(string name){
      Name = name ?? throw new ArgumentNullException(nameof(name));
    }
  }
}
