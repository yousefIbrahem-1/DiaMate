using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoFootUlcerImage
    {

        [Required]
        public int PatientId { get; set; }

        [Required]
        public byte[] Image { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Required,MaxLength(250)]
        public string Ai_detectionResult { get; set; }

        
        [Required,Column(TypeName = "decimal(5,2)")]
        public decimal? AIConfidence { get; set; } = null;

        [MaxLength(500)]
        public string? Notes { get; set; } = null;
    }
}
