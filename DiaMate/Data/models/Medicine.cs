using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

       
        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
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
