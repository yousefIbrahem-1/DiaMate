using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoMeal
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Read_date { get; set; } = DateTime.Now;


        [Required, Column(TypeName = "decimal(6,2)")]
        public decimal Calories { get; set; }

        [Required]
        public decimal protein { get; set; }

        [Required]
        public decimal carbs { get; set; }

        [Required]
        public decimal fats { get; set; }

        public string? Notes { get; set; }
    }
}
