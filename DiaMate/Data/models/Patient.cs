using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public enum enDiabetesType
    {
        Type1 = 1,
        Type2 = 2,
        Gestational = 3,
        Prediabetes = 4,
    }
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

         [Required]
        [DateRangeWithAge(0, 70)]
        public DateTime DateOfDiagnosis { get; set; }


        [Required, Range(1, 4,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short DiabetesType { get; set; } 

        [Required,Range(40, 200,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Weight { get; set; }

        [Required, Range(100, 230,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Height { get; set; } 

        public byte[]? QrCodeBase64 { get; set; }

        public string? Notes {  get; set; }=null;

      
        public int PersonId { set; get; }

        [Required, ForeignKey(nameof(PersonId))]
        public Person Person { set; get; }

        public AppUser UserAccount { get; set; }

        public ICollection<BloodGlucoseReading> BloodGlucoseReadings { get; set; } = new List<BloodGlucoseReading >();
        public ICollection<LabTest> LabTests { get; set; } = new List<LabTest>();
        public ICollection<FootUlcerImage> FootUlcerImages { get; set; } = new List<FootUlcerImage>();
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();


    }
}
