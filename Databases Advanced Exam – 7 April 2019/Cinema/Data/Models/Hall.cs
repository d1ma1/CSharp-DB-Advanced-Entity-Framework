﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Data.Models
{
    public class Hall
    {
        public Hall()
        {
            Projections = new List<Projection>();
            Seats = new List<Seat>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; }

        public ICollection<Seat> Seats { get; set; }
    }

//    •	Id – integer, Primary Key
//•	Name – text with length[3, 20] (required)
//•	Is4Dx - bool
//•	Is3D - bool
//•	Projections - collection of type Projection
//•	Seats - collection of type Seat

}
