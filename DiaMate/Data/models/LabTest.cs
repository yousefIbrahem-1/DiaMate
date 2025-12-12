using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class LabTest
    {
        [Key]
        public int TestId { get; set; }

        [Required,ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        
        public Patient Patient { get; set; }

        [Required,MaxLength(150)]
        public string TestName { get; set; }

        [Required, MaxLength(500)]
        public double Result_value { get; set; }

        [Required, MaxLength(50)]
        public string NormalRange { get; set; }

        public DateTime TestDate { get; set; }= DateTime.Now;

        [Required]
        public byte[] Report_Image { get; set; }

        public string? Notes { get; set; }
    }
}
