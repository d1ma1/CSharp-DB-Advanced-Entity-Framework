using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PetClinic.DataProcessor.Imports
{
   public  class ImportAnimalDto
    {
        [MaxLength(20), MinLength(3)]
        [Required]
        public string Name { get; set; }

        [MaxLength(20), MinLength(3)]
        [Required]
        public string Type { get; set; }

        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        public DtoPassport Passport { get; set; }
    }

    public class DtoPassport
    {
        [Key]
        [RegularExpression(@"^[a-zA-Z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }

        [MaxLength(30), MinLength(3)]
        [Required]
        public string OwnerName { get; set; }

        [Required]
        [RegularExpression(@"(^\+359[0-9]{9}$)|(^0[0-9]{9}$)")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }

    //"Name":"Bella",
    //"Type":"cat",
    //"Age": 2,
    //"Passport": {
    //  "SerialNumber": "etyhGgH678",
    //  "OwnerName": "Sheldon Cooper",
    //  "OwnerPhoneNumber": "0897556446",
    //  "RegistrationDate": "12-03-2014"
    //}

}
