using System.ComponentModel.DataAnnotations;

namespace DiaMate.dtoModels
{
    public class dtoLoginByEmail
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
