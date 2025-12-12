using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoMedicine
    {
        [Required]
        public int PatientId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Dosage { get; set; }

        [Required,MaxLength(100)]
        public string Frequency { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
