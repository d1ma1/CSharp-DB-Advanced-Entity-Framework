using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Imports
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [XmlElement("Vet")]
        public string VetName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{7}[0-9]{3}$")]
        [XmlElement("Animal")]
        public string AnimalSerialNumber { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string ProcedureDate { get; set; }

        public ProcedureAnimalAidDto[] AnimalAids { get; set; }
    }
}

[XmlType("AnimalAid")]
public class ProcedureAnimalAidDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(30)]
    public string Name { get; set; }
}

    //<Procedure>
    //     <Vet>Niels Bohr</Vet>
    //     <Animal>acattee321</Animal>
    //      <DateTime>14-01-2016</DateTime>
    //     <AnimalAids>
    //         <AnimalAid>
    //             <Name>Nasal Bordetella</Name>
    //         </AnimalAid>
    //         <AnimalAid>
    //             <Name>Internal Deworming</Name>
    //         </AnimalAid>
    //         <AnimalAid>
    //             <Name>Fecal Test</Name>
    //         </AnimalAid>
    //     </AnimalAids>
    // </Procedure>


