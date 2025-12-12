using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public class AppUser:IdentityUser
    {
       

        [Required,ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        
        public Patient Patient { get; set; }

       

    }
}
