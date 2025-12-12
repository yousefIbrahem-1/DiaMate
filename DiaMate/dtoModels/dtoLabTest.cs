using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoLabTest
    {


        [Required]
        public int PatientId { get; set; }

        [MaxLength(150)]
        public string TestName { get; set; }

        [MaxLength(500)]
        public double Result_value { get; set; }

        [MaxLength(50)]
        public string NormalRange { get; set; }

        public string? Notes { get; set; }

        public DateTime TestDate { get; set; } = DateTime.Now;
        [Required]
        public byte[] Report_Image { get; set; }
    }
}
