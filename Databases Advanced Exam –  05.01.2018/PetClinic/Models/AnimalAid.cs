using System;
using System.Collections.Generic;
using System.Text;

namespace PetClinic.Models
{
    public class AnimalAid
    {
        public AnimalAid()
        {
            AnimalAidProcedures = new List<ProcedureAnimalAid>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<ProcedureAnimalAid> AnimalAidProcedures { get; set; }
    }

//    -	Id – integer, Primary Key
//-	Name – text with min length 3 and max length 30 (required, unique)
//-	Price – decimal (non-negative, minimum value: 0.01, required)
//-	AnimalAidProcedures – collection of type ProcedureAnimalAid

}
