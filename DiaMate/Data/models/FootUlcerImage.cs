using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class FootUlcerImage
    {
        [Key]
        public int ImageId { get; set; }

        
        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Required, MaxLength(250)]
        public string Ai_detectionResult { get; set; }

        [Required,Column(TypeName = "decimal(5,2)")]
        public decimal? AIConfidence { get; set; }

        [ MaxLength(500)]
        public string? Notes { get; set; } 

        
    }
}
