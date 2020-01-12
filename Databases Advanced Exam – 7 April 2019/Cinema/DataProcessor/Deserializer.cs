namespace Cinema.DataProcessor
{
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var dtoMovies = JsonConvert.DeserializeObject<ImportMoviesDto[]>(jsonString);

            var sb = new StringBuilder();

            var validMovies = new List<Movie>();

            var titles = new List<string>();

            foreach (var dtoMovie in dtoMovies)
            {
               
                if (titles.Contains(dtoMovie.Title) || !IsValid(dtoMovie))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var movie = new Movie();
                movie.Title = dtoMovie.Title;
                movie.Genre = Enum.Parse<Genre>(dtoMovie.Genre);
                movie.Duration = TimeSpan.Parse(dtoMovie.Duration);
                movie.Rating = dtoMovie.Rating;
                movie.Director = dtoMovie.Director;

                sb.AppendLine($"Successfully imported {dtoMovie.Title} with genre {dtoMovie.Genre} and rating {dtoMovie.Rating:F2}!");
                                
                validMovies.Add(movie);
                titles.Add(dtoMovie.Title);
            }

            context.Movies.AddRange(validMovies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var dtoHalls = JsonConvert.DeserializeObject<ImportHallsAndSeats[]>(jsonString);

            var sb = new StringBuilder();

            var validHalls = new List<Hall>();

            foreach (var dtoHall in dtoHalls)
            {
                if (!IsValid(dtoHall))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var hall = new Hall();

                hall.Name = dtoHall.Name;
                hall.Is3D = dtoHall.Is3D;
                hall.Is4Dx = dtoHall.Is4Dx;

                for (int i = 0; i < dtoHall.Seats; i++)
                {
                    var seat = new Seat();
                    hall.Seats.Add(seat);
                }

                var ff = "";
                if (hall.Is4Dx)
                {
                    ff = "4Dx";
                }
                if (hall.Is3D)
                {
                    ff = "3D";
                }
                if (hall.Is3D && hall.Is4Dx)
                {
                    ff = "4Dx/3D";
                }
                if (!hall.Is3D && !hall.Is4Dx)
                {
                    ff = "Normal";
                }

                validHalls.Add(hall);
                sb.AppendLine($"Successfully imported {hall.Name}({ff}) with {hall.Seats.Count} seats!");

            }
            context.Halls.AddRange(validHalls);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportProjectionsDto[]), new XmlRootAttribute("Projections"));
            var dtoProjections = (ImportProjectionsDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validProjections = new List<Projection>();

            foreach (var dtoProjection in dtoProjections)
            {
                var hall = context.Halls.Find(dtoProjection.HallId);
                var movie = context.Movies.Find(dtoProjection.MovieId);

                if (!IsValid(dtoProjection) || hall==null || movie==null)
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var projection = new Projection();
                projection.Movie = movie;
                projection.Hall = hall;
                projection.DateTime = DateTime.ParseExact(dtoProjection.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                var currentDateTimeString = projection.DateTime.ToString("MM/dd/yyyy");

                validProjections.Add(projection);
                sb.AppendLine($"Successfully imported projection {projection.Movie.Title} on {currentDateTimeString}!");
                
            }

            context.Projections.AddRange(validProjections);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {

            var serializer = new XmlSerializer(typeof(ImportTicketsDto[]), new XmlRootAttribute("Customers"));
            var dtoCustomers = (ImportTicketsDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validCustomers = new List<Customer>();

            foreach (var dtoCustomer in dtoCustomers)
            {

                if (!IsValid(dtoCustomer))
                {
                    sb.AppendLine("Invalid data!");
                    continue;
                }

                var customer = new Customer();

                customer.FirstName = dtoCustomer.FirstName;
                customer.LastName = dtoCustomer.LastName;
                customer.Age = dtoCustomer.Age;
                customer.Balance = dtoCustomer.Balance;

                //var serializerTickets = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
                //var dtoTickets = (TicketDto[])serializer.Deserialize(new StringReader(xmlString));

                foreach (var dtoTicket in dtoCustomer.Tickets)
                {
                    var ticket = new Ticket();
                    ticket.Price = dtoTicket.Price;
                    ticket.Projection = context.Projections.Find(dtoTicket.ProjectionId);

                    customer.Tickets.Add(ticket);
                }

                validCustomers.Add(customer);
                sb.AppendLine($"Successfully imported customer {customer.FirstName} {customer.LastName} with bought tickets: {customer.Tickets.Count}!");
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
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