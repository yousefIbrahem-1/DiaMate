using DiaMate.Data;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        public MedicineController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetAllMedicineForPatient(int PatientId)
        {
            var patient=await _db.Patients.FirstOrDefaultAsync(p=>p.PatientId==PatientId);

            if (patient != null)
            {
                var Medicines=await _db.Medicines.Where(m=>m.PatientId==PatientId).ToListAsync();
                ICollection<dtoMedicine>dtoMedicines = new List<dtoMedicine>();
                foreach (var m in Medicines)
                {
                    dtoMedicines.Add(new dtoMedicine()
                    {
                        PatientId = m.PatientId,
                        Name= m.Name,
                        Dosage= m.Dosage,
                        Frequency= m.Frequency,
                        StartDate= m.StartDate,
                        EndDate= m.EndDate,
                        Notes= m.Notes

                    });
                }
                return Ok(dtoMedicines);
            }
            else
            {
                return NotFound("this patient is not exist");
            }
        }

       
    }
}
