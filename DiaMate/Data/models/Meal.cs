using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        [Required,ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        
        public Patient Patient { get; set; }

        
        public DateTime Read_date { get; set; }= DateTime.Now;


        [Column(TypeName = "decimal(6,2)")]
        public decimal Calories { get; set; }


        public string? Notes { get; set; }
    }
}
