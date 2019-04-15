using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;

namespace Cinema.Data.Models
{
    public class Movie
    {
        public Movie()
        {
            Projections = new List<Projection>();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public Genre Genre { get; set; }

        public TimeSpan Duration { get; set; }

        public double Rating { get; set; }

        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; }
    }

//    •	Id – integer, Primary Key
//•	Title – text with length[3, 20] (required)
//•	Genre – enumeration of type Genre, with possible values(Action, Drama, Comedy, Crime, Western, Romance, Documentary, Children, Animation, Musical) (required)
//•	Duration – TimeSpan(required)
//•	Rating – double in the range[1, 10] (required)
//•	Director – text with length[3, 20] (required)
//•	Projections - collection of type Projection

}
