using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DiaMate.Data.models
{
    public enum enGender { Male = 0, Female = 1 }
    public class Person
    {
        
        [Key]
        public int PersonID { set; get; }

        [Required,MaxLength(50)]
        public string FirstName { set; get; }

        [Required, MaxLength(50)]
        public string SecondName { set; get; }

        [MaxLength(50)]
        public string? ThirdName { set; get; } = null;

        [MaxLength(50)]
        public string? LastName { set; get; } = null;

        [MaxLength(250)]
        public string FullName
        {
            
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        [Required,Range(typeof(DateTime), "1/1/1900", "1/1/2010",
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime DateOfBirth { set; get; }

        public int Age
        {
            get { return DateTime.Now.Year - DateOfBirth.Year; } 
        }

        [Required,Range(0, 1,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short Gender { set; get; }= (short)enGender.Male;

        public string? Address { set; get; }= null;

        [Required,MinLength(11),MaxLength(11)]
        public string Phone { set; get; }

        [MaxLength(11)]
        public string? HomePhone { set; get; }=null;

        [Required,DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        public byte[]? ProfileImage { set; get; } = null;

        


       

    }
}
