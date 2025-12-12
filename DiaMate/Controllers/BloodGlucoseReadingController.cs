using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BloodGlucoseReadingController : ControllerBase
    {
        public BloodGlucoseReadingController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAllReadingsForPatient(int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var BloodGlucoseReadings = await _db.BloodGlucoseReadings.Where(B => B.PatientId == id).ToListAsync();
                    ICollection<dtoBloodGlucoseReading> dtoBloodGlucoseReadings = new List<dtoBloodGlucoseReading>();
                    foreach (var reading in BloodGlucoseReadings)
                    {
                        dtoBloodGlucoseReadings.Add(new dtoBloodGlucoseReading
                        {
                            PatientId = reading.PatientId,
                            reading_value = reading.reading_value,
                            MeasurementTime = reading.MeasurementTime,
                            MeasurementType = reading.MeasurementType,
                            Notes = reading.Notes
                        });
                    }

                    return Ok(dtoBloodGlucoseReadings);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }



        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTheLastReadingForPatient(int id)
        {
           if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var lastReading = await _db.BloodGlucoseReadings
                                     .Where(r => r.PatientId == id)
                                     .OrderByDescending(r => r.MeasurementTime)   
                                     .FirstOrDefaultAsync();


                    dtoBloodGlucoseReading dtoBloodGlucoseReading= new dtoBloodGlucoseReading()
                        {
                            PatientId = lastReading.PatientId,
                            reading_value = lastReading.reading_value,
                            MeasurementTime = lastReading.MeasurementTime,
                            MeasurementType =lastReading.MeasurementType,
                            Notes = lastReading.Notes
                        };
                   

                    return Ok(dtoBloodGlucoseReading);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetReadingsByDateForPatient(int id,DateTime readingDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var Readings = await _db.BloodGlucoseReadings
                                     .Where(r => (r.PatientId == id&& (r.MeasurementTime.Year == readingDate.Year &&
                                                                        r.MeasurementTime.Month == readingDate.Month &&
                                                                        r.MeasurementTime.Day == readingDate.Day)))
                                     .ToListAsync();

                    ICollection<dtoBloodGlucoseReading> dtoBloodGlucoseReadings = new List<dtoBloodGlucoseReading>();
                    foreach (var reading in Readings)
                    {
                        dtoBloodGlucoseReadings.Add(new dtoBloodGlucoseReading
                        {
                            PatientId = reading.PatientId,
                            reading_value = reading.reading_value,
                            MeasurementTime = reading.MeasurementTime,
                            MeasurementType = reading.MeasurementType,
                            Notes = reading.Notes
                        });
                    }



                   return Ok(dtoBloodGlucoseReadings);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetReadingsByDateRangeForPatient(int id, DateTime FromReadingDate,DateTime ToReadingDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var Readings = await _db.BloodGlucoseReadings
                                     .Where(r => (r.PatientId == id && (r.MeasurementTime>=FromReadingDate && r.MeasurementTime<=ToReadingDate)))
                                     .ToListAsync();

                    ICollection<dtoBloodGlucoseReading> dtoBloodGlucoseReadings = new List<dtoBloodGlucoseReading>();
                    foreach (var reading in Readings)
                    {
                        dtoBloodGlucoseReadings.Add(new dtoBloodGlucoseReading
                        {
                            PatientId = reading.PatientId,
                            reading_value = reading.reading_value,
                            MeasurementTime = reading.MeasurementTime,
                            MeasurementType = reading.MeasurementType,
                            Notes = reading.Notes
                        });
                    }



                    return Ok(dtoBloodGlucoseReadings);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddReadingForPatient(dtoBloodGlucoseReading dtoBloodGlucoseReading)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == dtoBloodGlucoseReading.PatientId)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == dtoBloodGlucoseReading.PatientId);
                    if (Patient != null)
                    {
                        BloodGlucoseReading bloodGlucoseReading = new BloodGlucoseReading()
                        {
                            PatientId = dtoBloodGlucoseReading.PatientId,
                            reading_value = dtoBloodGlucoseReading.reading_value,
                            MeasurementTime = dtoBloodGlucoseReading.MeasurementTime,
                            MeasurementType = dtoBloodGlucoseReading.MeasurementType,
                            Notes = dtoBloodGlucoseReading.Notes

                        };
                        await _db.BloodGlucoseReadings.AddAsync(bloodGlucoseReading);
                        _db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound("Patient is not exist");
                    }
                }
                return BadRequest(ModelState);
            }
            return Unauthorized();
        }

    }
}
