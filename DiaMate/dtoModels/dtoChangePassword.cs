using System.ComponentModel.DataAnnotations;

namespace DiaMate.dtoModels
{
    public class dtoChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required] 
        public string NewPassword {  get; set; }
    }
}
