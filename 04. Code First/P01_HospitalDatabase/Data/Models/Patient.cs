﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {

        public Patient()
        {
            Visitations = new List<Visitation>();
            Diagnoses = new List<Diagnose>();
            Prescriptions = new List<PatientMedicament>();
        }
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }
        public ICollection<Visitation> Visitations { get; set; }
        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
