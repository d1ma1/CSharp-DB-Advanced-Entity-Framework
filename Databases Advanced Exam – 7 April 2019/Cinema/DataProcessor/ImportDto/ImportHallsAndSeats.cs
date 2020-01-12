using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportHallsAndSeats
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Seats { get; set; }
    }

    //    "Name": "Methocarbamol",
    //"Is4Dx": false,
    //"Is3D": true,
    //"Seats": 52

}
