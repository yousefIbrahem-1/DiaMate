using DiaMate.Data.models;
using DiaMate.Migrations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoPatient
    {
    
        [Required, MaxLength(50)]
        public string FirstName { set; get; }


        [MaxLength(50)]
        public string? LastName { set; get; } = null;


        [Required, Range(typeof(DateTime), "1/1/1900", "1/1/2010",
       ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime DateOfBirth { set; get; }


        [Required, Range(0, 1,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short Gender { set; get; } = (short)enGender.Male;

        public string? Address { set; get; } = null;

        [Required, MinLength(11), MaxLength(11)]
        public string Phone { set; get; }

        [MaxLength(11)]
        public string? HomePhone { set; get; } = null;

        [Required]
        public string Email { set; get; }

        public byte[]? ProfileImage { set; get; }=null;

        [Required, Range(1, 5,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public DateTime DateOfDiagnosis { get; set; }

        public short DiabetesType { get; set; } = (short)enDiabetesType.Type1;

        [Required, Range(40, 200,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Weight { get; set; }

        [Required, Range(100, 230,
       ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Height { get; set; } = 150.00;

        public string? Notes { get; set; } = null;

       //public ICollection<dtoBloodGlucoseReading> bloodGlucoseReadings = new List<dtoBloodGlucoseReading>();
       //public ICollection<dtoFootUlcerImage> footUlcerImages=new List<dtoFootUlcerImage>();
    }
}
