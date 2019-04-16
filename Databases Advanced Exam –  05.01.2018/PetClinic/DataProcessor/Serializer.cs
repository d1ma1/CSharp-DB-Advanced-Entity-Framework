namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.EntityFrameworkCore.Extensions.Internal;
    using Newtonsoft.Json;
    using PetClinic.Data;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var result = context.Passports
                .Where(x => x.OwnerPhoneNumber == phoneNumber)
                .OrderBy(x => x.Animal.Age)
                .ThenBy(x => x.SerialNumber)
                .Select( x => new {
                    OwnerName = x.OwnerName,
                    AnimalName = x.Animal.Name,
                    Age = x.Animal.Age,
                    SerialNumber = x.SerialNumber,
                    RegisteredOn = x.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToArray();


            var json = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            return json;
        }


        public static string ExportAllProcedures(PetClinicContext context)
        {
            //var procedures = context.Procedures.Select({
            //    e =>  new ExportProceduresDto()
            //    {
            //        e.N
            //    }
            //})
            //var xml = SerializeObject(procedures, "Customers");
            return string.Empty;
        }

        public static string SerializeObject<T>(T values, string rootName, bool omitXmlDeclaration = false, bool indentXml = true)
        {
            string xml = string.Empty;

            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            var settings = new XmlWriterSettings()
            {
                Indent = indentXml,
                OmitXmlDeclaration = omitXmlDeclaration
            };

            XmlSerializerNamespaces @namespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, values, @namespace);
                xml = stream.ToString();
            }

            return xml;
        }

    }
}
