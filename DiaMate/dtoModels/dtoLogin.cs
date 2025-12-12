using System.ComponentModel.DataAnnotations;

namespace DiaMate.dtoModels
{
    public class dtoLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

       
    }
}
