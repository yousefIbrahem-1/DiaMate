using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Data.models
{
    public enum NotificationType { Reminder=0, Warning=1, Info=2 }
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required, ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }

        
        public Patient Patient { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }

        public short Type { get; set; }=(short)NotificationType.Info;

        public DateTime Reminder_date { get; set; }=DateTime.Now; 

        

    }
}
