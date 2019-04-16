using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Imports
{
    [XmlType("Vet")]
    public class ImportVetDto
    {
        [MaxLength(40), MinLength(3)]
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [MaxLength(50), MinLength(3)]
        [Required]
        [XmlElement("Profession")]
        public string Profession { get; set; }

        [Range(22, 65)]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"(^\+359[0-9]{9}$)|(^0[0-9]{9}$)")]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }

    //Vets>
    //<Vet>
    //    <Name>Michael Jordan</Name>
    //    <Profession>Emergency and Critical Care</Profession>
    //    <Age>45</Age>
    //    <PhoneNumber>0897665544</PhoneNumber>
    //</Vet>

}
