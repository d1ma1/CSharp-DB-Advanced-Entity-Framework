using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public  class ImportMoviesDto
    {
        [MaxLength(20), MinLength(3)]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }

        [MaxLength(20), MinLength(3)]
        public string Director { get; set; }
    }

    //    "Title": "Gui Si (Silk)",
    //"Genre": "Drama",
    //"Duration": "02:21:00",
    //"Rating": 9,
    //"Director": "Perl Swyne"

}
