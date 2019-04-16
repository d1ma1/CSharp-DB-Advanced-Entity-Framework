using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Models
{
    public class Animal
    {
        public Animal()
        {
            Procedures = new List<Procedure>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Age { get; set; }

        [ForeignKey(nameof(Passport))]
        public string PassportSerialNumber { get; set; }
        public Passport Passport { get; set; }

        public ICollection<Procedure> Procedures { get; set; }
    }

//    -	Id – integer, Primary Key
//-	Name – text with min length 3 and max length 20 (required)
//-	Type – text with min length 3 and max length 20 (required)
//-	Age – integer, cannot be negative or 0 (required)
//-	PassportSerialNumber ¬– string, foreign key
//-	Passport – the passport of the animal(required)
//-	Procedures – the procedures, performed on the animal

}
