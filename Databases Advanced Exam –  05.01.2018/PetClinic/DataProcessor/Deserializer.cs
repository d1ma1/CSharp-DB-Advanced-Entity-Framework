namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Imports;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var dtoAids = JsonConvert.DeserializeObject<ImportAnimalAidsDto[]>(jsonString);
            var sb = new StringBuilder();
            var validAids = new List<AnimalAid>();

            var names = new List<string>();

            foreach (var dtoAid in dtoAids)
            {
                if (names.Contains(dtoAid.Name) || !IsValid(dtoAid))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var aid = new AnimalAid();
                aid.Name = dtoAid.Name;
                aid.Price = dtoAid.Price;

                validAids.Add(aid);
                sb.AppendLine($"Record {aid.Name} successfully imported.");
                names.Add(aid.Name);
            }

            context.AnimalAids.AddRange(validAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var dtoAnimals = JsonConvert.DeserializeObject<ImportAnimalDto[]>(jsonString);
            var sb = new StringBuilder();
            var validAnimals = new List<Animal>();

            var validPassports = new List<Passport>();


            foreach (var dtoAnimal in dtoAnimals)
            {
                DateTime regDate;
                var isRegDateValid = DateTime.TryParse(dtoAnimal.Passport.RegistrationDate, out regDate);

                if (!IsValid(dtoAnimal) || !IsValid(dtoAnimal.Passport)
                    ||validPassports.Any(p => p.SerialNumber.Equals(dtoAnimal.Passport.SerialNumber, StringComparison.OrdinalIgnoreCase)))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var passport = new Passport
                {
                    SerialNumber = dtoAnimal.Passport.SerialNumber,
                    OwnerName = dtoAnimal.Passport.OwnerName,
                    OwnerPhoneNumber = dtoAnimal.Passport.OwnerPhoneNumber,
                    RegistrationDate = regDate
                };

                var animal = new Animal
                {
                    Name = dtoAnimal.Name,
                    Type = dtoAnimal.Type,
                    Age = dtoAnimal.Age,
                    Passport = passport,
                    PassportSerialNumber = dtoAnimal.Passport.SerialNumber,

                };

                validAnimals.Add(animal);
                sb.AppendLine($"Record {animal.Name} Passport №: {animal.PassportSerialNumber} successfully imported.");

                validPassports.Add(passport);
            }

            context.Animals.AddRange(validAnimals);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportVetDto[]), new XmlRootAttribute("Vets"));
            var dtoVets = (ImportVetDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var validVets = new List<Vet>();

            var phones = new List<string>();

            foreach (var dtoVet in dtoVets)
            {
                if (!IsValid(dtoVet) || phones.Contains(dtoVet.PhoneNumber))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = new Vet();
                vet.Name = dtoVet.Name;
                vet.Age = dtoVet.Age;
                vet.PhoneNumber = dtoVet.PhoneNumber;
                vet.Profession = dtoVet.Profession;

                validVets.Add(vet);
                sb.AppendLine($"Record {vet.Name} successfully imported.");
                phones.Add(vet.PhoneNumber);
            }

            context.Vets.AddRange(validVets);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            var deserializedProcedures = (ProcedureDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validProcedures = new List<Procedure>();

            foreach (var dto in deserializedProcedures)
            {
                DateTime procedureDate;
                var isProcDateValid = DateTime.TryParseExact(dto.ProcedureDate,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out procedureDate);

                if (!IsValid(dto) ||
                    dto.AnimalAids.Length != dto.AnimalAids.Select(a => a.Name).Distinct().Count())
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var vet = context.Vets
                    .FirstOrDefault(v => v.Name.Equals(dto.VetName, StringComparison.OrdinalIgnoreCase));
                if (vet == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = context.Animals
                    .FirstOrDefault(a => a.PassportSerialNumber.Equals(dto.AnimalSerialNumber, StringComparison.OrdinalIgnoreCase));
                if (animal == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                var aids = context.AnimalAids
                  .Where(a => dto.AnimalAids.Any(dtoAid => dtoAid.Name.Equals(a.Name, StringComparison.OrdinalIgnoreCase)))
                  .ToArray();
                if (aids.Length != dto.AnimalAids.Length)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var mappings = new List<ProcedureAnimalAid>();
                foreach (var dbAid in aids)
                {
                    mappings.Add(new ProcedureAnimalAid
                    {
                        AnimalAidId = dbAid.Id
                    });
                }

                var proc = new Procedure
                {
                    VetId = vet.Id,
                    AnimalId = animal.Id,
                    DateTime = procedureDate,
                    ProcedureAnimalAids = mappings
                };

                validProcedures.Add(proc);

                sb.AppendLine("Record successfully imported.");
            }
            context.Procedures.AddRange(validProcedures);
            context.SaveChanges();

            var result = sb.ToString();
            return result;
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }

    }
}
