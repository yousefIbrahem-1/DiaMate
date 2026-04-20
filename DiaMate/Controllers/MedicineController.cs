using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicineController : ControllerBase
    {
        public MedicineController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetAllMedicinesForPatient(int PatientId)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Medicines = await _db.Medicines.Where(m => m.PatientId == PatientId).ToListAsync();
                    ICollection<dtoMedicine> dtoMedicines = new List<dtoMedicine>();
                    foreach (var m in Medicines)
                    {
                        dtoMedicines.Add(new dtoMedicine()
                        {
                            PatientId = m.PatientId,
                            Name = m.Name,
                            Dosage = m.Dosage,
                            Frequency = m.Frequency,
                            StartDate = m.StartDate,
                            EndDate = m.EndDate,
                            Notes = m.Notes

                        });
                    }
                    return Ok(dtoMedicines);
                }
                else
                {
                    return NotFound("message: this patient is not exist");
                }
            }
            return Unauthorized();
        }
        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetTheLastMedicineForPatient(int PatientId)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Medicine = await _db.Medicines
                        .Where(m => m.PatientId == PatientId)
                        .OrderByDescending(m => m.MedicineId)
                        .FirstOrDefaultAsync();

                    dtoMedicine dtoMedicines = new dtoMedicine()
                    {

                        PatientId = Medicine.PatientId,
                        Name = Medicine.Name,
                        Dosage = Medicine.Dosage,
                        Frequency = Medicine.Frequency,
                        StartDate = Medicine.StartDate,
                        EndDate = Medicine.EndDate,
                        Notes = Medicine.Notes


                    };

                    return Ok(dtoMedicines);
                }
                else
                {
                    return NotFound("message: this patient is not exist");
                }
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetTheMedicineByDateForPatient(int PatientId,DateTime startDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Medicines = await _db.Medicines
                        .Where(m => m.PatientId == PatientId &&
                        (m.StartDate.Year == startDate.Year &&
                         m.StartDate.Month == startDate.Month &&
                         m.StartDate.Day == startDate.Day))
                        .ToListAsync();

                    ICollection<dtoMedicine> dtoMedicines = new List<dtoMedicine>();
                    foreach (var m in Medicines)
                    {
                        dtoMedicines.Add(new dtoMedicine()
                        {
                            PatientId = m.PatientId,
                            Name = m.Name,
                            Dosage = m.Dosage,
                            Frequency = m.Frequency,
                            StartDate = m.StartDate,
                            EndDate = m.EndDate,
                            Notes = m.Notes

                        });
                    }

                    return Ok(dtoMedicines);
                }
                else
                {
                    return NotFound("message: this patient is not exist");
                }
            }
            return Unauthorized();
        }


        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetTheMedicineByDateRangeForPatient(int PatientId, DateTime FromDate,DateTime ToDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Medicines = await _db.Medicines
                        .Where(m => m.PatientId == PatientId &&
                        (m.StartDate >= FromDate && m.StartDate <= ToDate))
                        .ToListAsync();

                    ICollection<dtoMedicine> dtoMedicines = new List<dtoMedicine>();
                    foreach (var m in Medicines)
                    {
                        dtoMedicines.Add(new dtoMedicine()
                        {
                            PatientId = m.PatientId,
                            Name = m.Name,
                            Dosage = m.Dosage,
                            Frequency = m.Frequency,
                            StartDate = m.StartDate,
                            EndDate = m.EndDate,
                            Notes = m.Notes

                        });
                    }

                    return Ok(dtoMedicines);
                }
                else
                {
                    return NotFound("message: this patient is not exist");
                }
            }
            return Unauthorized();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddNewMedicine(dtoMedicine dtoMedicine)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == dtoMedicine.PatientId)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == dtoMedicine.PatientId);
                    if (Patient != null)
                    {
                        Medicine medicine = new Medicine()
                        {
                            PatientId = dtoMedicine.PatientId,
                            Name = dtoMedicine.Name,
                            Dosage = dtoMedicine.Dosage,
                            Frequency = dtoMedicine.Frequency,
                            StartDate = dtoMedicine.StartDate,
                            EndDate = dtoMedicine.EndDate,
                            Notes = dtoMedicine.Notes
                        };
                        await _db.Medicines.AddAsync(medicine);
                        _db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound("message: Patient is not exist");
                    }
                }
                return BadRequest($"message: {ModelState}");
            }
            return Unauthorized();
        }
    }
}
