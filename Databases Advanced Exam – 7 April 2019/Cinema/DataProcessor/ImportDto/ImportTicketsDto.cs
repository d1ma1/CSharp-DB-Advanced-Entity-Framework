using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportTicketsDto
    {
        [MaxLength(20), MinLength(3)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [MaxLength(20), MinLength(3)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Range(12, 110)]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        public List<TicketDto> Tickets { get; set; }

        //        <Customer>
        //  <FirstName>Randi</FirstName>
        //  <LastName>Ferraraccio</LastName>
        //  <Age>20</Age>
        //  <Balance>59.44</Balance>
        //  <Tickets>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>7</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>15</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>12.13</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>11</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>9.13</Price>
        //    </Ticket>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>9.13</Price>
        //    </Ticket>
        //  </Tickets>
        //</Customer>

    }
}
