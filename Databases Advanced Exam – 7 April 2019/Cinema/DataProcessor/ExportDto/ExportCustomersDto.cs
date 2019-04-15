using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class ExportCustomersDto
    {
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

        [XmlElement("SpentMoney")]
        public string SpentMoney { get; set; }

        [XmlElement("SpentTime")]
        public string SpentTime { get; set; }
    }

  //  <Customers>
  //<Customer FirstName = "Marjy" LastName="Starbeck">
  //  <SpentMoney>82.65</SpentMoney>
  //  <SpentTime>17:04:00</SpentTime>
  //</Customer>

}
