using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [Required]
        public string Name { get; set; }


        public DateTime Read_date { get; set; }= DateTime.Now;


        [Required,Column(TypeName = "decimal(6,2)")]
        public decimal Calories { get; set; }

        [Required]
        public decimal protein { get; set; }

        [Required]
        public decimal carbs { get; set; }

        [Required]
        public decimal fats { get; set; }

        public string? Notes { get; set; }

        
            //"calories": 950,
            //"protein": 70,
            //"fats": 40,
            //"carbs": 180,
            //"createdAt": "2026-02-23T10:30:00"
    }
}
