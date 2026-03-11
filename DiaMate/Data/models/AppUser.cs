using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class AppUser:IdentityUser
    {
       

     
        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

       

    }
}
