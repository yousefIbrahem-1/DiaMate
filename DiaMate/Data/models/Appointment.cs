using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DiaMate.Data.models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

      
        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [Required]
        public string Doctor { get; set; }

        public string? Notes { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}
