using System.ComponentModel.DataAnnotations;

namespace DiaMate.dtoModels
{
    public class dtoLoginByUsername
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

       
    }
}
