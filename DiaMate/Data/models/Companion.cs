using DiaMate.Data.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiaMate.Models
{
    public enum enRelationshipType
{
    Father,
    Mother,
    Husband,
    Wife,
    Brother,
    Sister,
    Friend,
    Other
}
    public class Companion
    {
        [Key]
        public int Id { get; set; }

        public enRelationshipType Relationship { get; set; } = enRelationshipType.Friend;

        public int PersonId { set; get; }

        [Required, ForeignKey(nameof(PersonId))]
        public Person Person { set; get; }

        public int PatientId { get; set; }

        [Required, ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }
    }
}