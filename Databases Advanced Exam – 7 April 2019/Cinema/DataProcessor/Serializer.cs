namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {

            var movies = context.Movies.Where(x => x.Rating >= rating && x.Projections.SelectMany(t => t.Tickets).Any())
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = $"{m.Rating:F2}",
                    TotalIncomes = m.Projections.Select(p => p.Tickets.Sum(t => t.Price)).Sum().ToString("f2"),
                    Customers = m.Projections
                       .SelectMany(p => p.Tickets.Select(t => t.Customer))
                       .Select(x => new
                       {
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           Balance = x.Balance.ToString("f2")
                       })
                       .OrderByDescending(c => c.Balance)
                       .ThenBy(c => c.FirstName)
                       .ThenBy(c => c.LastName)
                       .ToArray()
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToArray();


            var json = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers.Where(x => x.Age >= age)
                .Include(x => x.Tickets)
                .OrderByDescending(y => y.Tickets.Sum(p => p.Price))
                .Select(x => new ExportCustomersDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = x.Tickets.Sum(p => p.Price).ToString("f2"),
                    SpentTime = (new TimeSpan(x.Tickets.Sum(r => r.Projection.Movie.Duration.Ticks))).ToString()
                })
               
                .Take(10)
                .ToList();

            var xml = SerializeObject(customers, "Customers");
            return xml;
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