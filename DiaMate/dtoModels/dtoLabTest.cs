using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoLabTest
    {


        [Required]
        public int PatientId { get; set; }

        [Required,MaxLength(150)]
        public string TestName { get; set; }

        [Required]
        public double Result_value { get; set; }

        [Required,MaxLength(50)]
        public string NormalRange { get; set; }


        public DateTime TestDate { get; set; } = DateTime.Now;

        
        public byte[]? Report_Image { get; set; }

        public string? Notes { get; set; }
    }
}
