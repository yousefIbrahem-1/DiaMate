using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoMeal
    {
        [Required]
        public int PatientId { get; set; }


        public DateTime Read_date { get; set; } = DateTime.Now;


        [Column(TypeName = "decimal(6,2)")]
        public decimal Calories { get; set; }


        public string? Notes { get; set; }
    }
}
