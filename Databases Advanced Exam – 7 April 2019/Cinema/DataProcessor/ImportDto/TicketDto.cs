﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Ticket")]
    public class TicketDto
    {
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        //  <Tickets>
        //    <Ticket>
        //      <ProjectionId>1</ProjectionId>
        //      <Price>7</Price>
        //    </Ticket>
    }
}