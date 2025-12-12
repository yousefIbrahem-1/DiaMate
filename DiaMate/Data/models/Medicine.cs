using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required,ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        
        public Patient Patient { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required,MaxLength(50)]
        public string Dosage { get; set; } 

        [ Required, MaxLength(100)]
        public string Frequency { get; set; } 

        public DateTime StartDate { get; set; }= DateTime.Now;

        public DateTime? EndDate { get; set; }

        public string Notes { get; set; }
    }
}
