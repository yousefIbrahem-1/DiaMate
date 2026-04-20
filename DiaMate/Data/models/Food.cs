using System.ComponentModel.DataAnnotations;

namespace DiaMate.Data.models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double CaloriesPer100g { get; set; }
        public double Carbs_G { get; set; }
        public double Sugar_G { get; set; }
        public double Fiber_G { get; set; }
        public double Protein_G { get; set; }
        public double Fat_G { get; set; }
        
    }
}
