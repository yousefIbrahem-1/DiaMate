using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.dtoModels
{
    public class dtoNewUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { set; get; }

        [Required, MaxLength(50)]
        public string SecondName { set; get; }

        [MaxLength(50)]
        public string? ThirdName { set; get; } = null;
            
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

        public byte[]? ProfileImage { set; get; }= null;


        [Required]
        public double Weight { get; set; }

        public string? Notes { get; set; }= null;

        


    }    
}
