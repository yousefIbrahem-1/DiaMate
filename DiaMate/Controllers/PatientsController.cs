using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        public PatientsController( AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;
       

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPatient (int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.Include(p => p.Person).SingleOrDefaultAsync(p => p.PatientId == id);
               

                if (Patient != null)
                {
                    dtoPatient dtoPatient = new dtoPatient()
                    {
                        FirstName = Patient.Person.FirstName,
                        LastName = Patient.Person.LastName,
                        Gender = Patient.Person.Gender,
                        Phone = Patient.Person.Phone,
                        HomePhone = Patient.Person.HomePhone,
                        Address = Patient.Person.Address,
                        Email = Patient.Person.Email,
                        DateOfBirth = Patient.Person.DateOfBirth,
                        ProfileImage = Patient.Person.ProfileImage,

                        Height = Patient.Height,
                        Weight = Patient.Weight,
                        DateOfDiagnosis = Patient.DateOfDiagnosis,
                        DiabetesType = Patient.DiabetesType,
                        Notes = Patient.Notes
                    };
                    //var BloodGlucoseReading = await _db.BloodGlucoseReadings.Where(B => B.PatientId == id).ToListAsync();
                    //foreach(var reading in BloodGlucoseReading)
                    //{
                    //    dtoPatient.bloodGlucoseReadings.Add(new dtoBloodGlucoseReading
                    //    { PatientId = reading.PatientId,
                    //    reading_value=reading.reading_value,
                    //    MeasurementTime=reading.MeasurementTime,
                    //    MeasurementType=reading.MeasurementType,
                    //    Notes = reading.Notes
                    //    });
                    //}
                    
                    return Ok(dtoPatient);
                }
                else
                {
                    return NotFound("message: this patient not exist");
                }
            }
            return Unauthorized();
        }
        

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdatePatient(int id ,dtoPatient dtoPatient)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.Include(p => p.Person).SingleOrDefaultAsync(p => p.PatientId == id);
                    if (Patient != null)
                    {

                        Patient.Person.FirstName = dtoPatient.FirstName;
                        Patient.Person.LastName = dtoPatient.LastName;
                        Patient.Person.Address = dtoPatient.Address;
                        Patient.Person.DateOfBirth = dtoPatient.DateOfBirth;
                        Patient.Person.Gender = dtoPatient.Gender;
                        Patient.Person.Email = dtoPatient.Email;
                        Patient.Person.Phone = dtoPatient.Phone;
                        Patient.Person.HomePhone = dtoPatient.HomePhone;
                        Patient.Person.ProfileImage = dtoPatient.ProfileImage;

                        Patient.DateOfDiagnosis = dtoPatient.DateOfDiagnosis;
                        Patient.DiabetesType = dtoPatient.DiabetesType;
                        Patient.Height = dtoPatient.Height;
                        Patient.Weight = dtoPatient.Weight;
                        Patient.Notes = dtoPatient.Notes;

                        _db.Patients.Update(Patient);
                        await _db.SaveChangesAsync();
                        return Ok(dtoPatient);

                    }
                    else
                    {
                        return NotFound("message: this Patient not exist");
                    }



                }
                return BadRequest($"message: {ModelState}");
            }
            return Unauthorized();
        }


    }
}
