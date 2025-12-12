using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public enum enMeasurementType { Fasting=0, BeforeMeal=1, AfterMeal=2, Random=3 }
                                 

    public class BloodGlucoseReading
    {
        [Key]
        public int ReadingId { get; set; }

        [Required,ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        
        public Patient Patient { get; set; }

        [Required,Column(TypeName = "decimal(6,2)")]
        public decimal reading_value { get; set; }

        public DateTime MeasurementTime { get; set; }= DateTime.Now;

        [Required, Range(0, 3,
         ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short MeasurementType { get; set; }= (short)enMeasurementType.Random;

        public string? Notes { get; set; }

    }
}
