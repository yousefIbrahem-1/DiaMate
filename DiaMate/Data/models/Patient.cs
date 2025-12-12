using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required,Range(40, 200,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Weight { get; set; }

        public string? Notes {  get; set; }=null;

        [Required,ForeignKey(nameof(Person))]
        public int PersonId { set; get; }

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
