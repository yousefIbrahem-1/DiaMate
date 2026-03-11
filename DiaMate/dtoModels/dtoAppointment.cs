using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoAppointment
    {

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string Doctor { get; set; }

        public string? Notes { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}
