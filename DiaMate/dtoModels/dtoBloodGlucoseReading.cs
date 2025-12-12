using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoBloodGlucoseReading
    {

        [Required]
        public int PatientId { get; set; }

        [Required, Column(TypeName = "decimal(6,2)")]
        public decimal reading_value { get; set; }

        public DateTime MeasurementTime { get; set; } = DateTime.Now;

        [Required, Range(0, 3,
         ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short MeasurementType { get; set; } = (short)enMeasurementType.Random;

        public string? Notes { get; set; } = null;
    }
}
