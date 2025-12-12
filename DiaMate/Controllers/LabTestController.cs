using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestController : ControllerBase
    {
        public LabTestController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAllTestsForPatient(int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var LabTests = await _db.LabTests
                                     .Where(t => t.PatientId == id)
                                     .ToListAsync();

                    ICollection<dtoLabTest> dtoLabTests = new List<dtoLabTest>();
                    foreach (var labTest in LabTests)
                    {
                        dtoLabTests.Add(new dtoLabTest()
                        {
                            PatientId = labTest.PatientId,
                            TestName = labTest.TestName,
                            Result_value = labTest.Result_value,
                            NormalRange = labTest.NormalRange,
                            TestDate = labTest.TestDate,
                            Notes = labTest.Notes,
                            Report_Image = labTest.Report_Image,
                        });
                    }

                    return Ok(dtoLabTests);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTheLastTestForPatient(int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var lastTest = await _db.LabTests
                                     .Where(t => t.PatientId == id)
                                     .OrderByDescending(t => t.TestDate)
                                     .FirstOrDefaultAsync();


                    dtoLabTest dtoLabTest = new dtoLabTest()
                    {
                        PatientId = lastTest.PatientId,
                        TestName = lastTest.TestName,
                        Result_value = lastTest.Result_value,
                        NormalRange = lastTest.NormalRange,
                        TestDate = lastTest.TestDate,
                        Notes = lastTest.Notes,
                        Report_Image = lastTest.Report_Image,
                    };


                    return Ok(dtoLabTest);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTheTestByDateForPatient(int id, DateTime TestDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var LabTests = await _db.LabTests
                                     .Where(t => t.PatientId == id &&
                                     (t.TestDate.Year == TestDate.Year &&
                                     t.TestDate.Month == TestDate.Month &&
                                     t.TestDate.Day == TestDate.Day))
                                     .ToListAsync();

                    ICollection<dtoLabTest> dtoLabTests = new List<dtoLabTest>();
                    foreach (var labTest in LabTests)
                    {
                        dtoLabTests.Add(new dtoLabTest()
                        {
                            PatientId = labTest.PatientId,
                            TestName = labTest.TestName,
                            Result_value = labTest.Result_value,
                            NormalRange = labTest.NormalRange,
                            TestDate = labTest.TestDate,
                            Notes = labTest.Notes,
                            Report_Image = labTest.Report_Image,
                        });
                    }

                    return Ok(dtoLabTests);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetTheTestByDateRangeForPatient(int id, DateTime fromTestDate, DateTime toTestDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var LabTests = await _db.LabTests
                                     .Where(t => t.PatientId == id && (t.TestDate >= fromTestDate && t.TestDate <= toTestDate))
                                     .ToListAsync();

                    ICollection<dtoLabTest> dtoLabTests = new List<dtoLabTest>();
                    foreach (var labTest in LabTests)
                    {
                        dtoLabTests.Add(new dtoLabTest()
                        {
                            PatientId = labTest.PatientId,
                            TestName = labTest.TestName,
                            Result_value = labTest.Result_value,
                            NormalRange = labTest.NormalRange,
                            TestDate = labTest.TestDate,
                            Notes = labTest.Notes,
                            Report_Image = labTest.Report_Image,
                        });
                    }

                    return Ok(dtoLabTests);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> AddNewLabTestForPatient(dtoLabTest dtoLabTest)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == dtoLabTest.PatientId)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == dtoLabTest.PatientId);
                    if (Patient != null)
                    {
                        LabTest labTest = new LabTest()
                        {
                            PatientId = dtoLabTest.PatientId,
                            TestName = dtoLabTest.TestName,
                            Result_value = dtoLabTest.Result_value,
                            NormalRange = dtoLabTest.NormalRange,
                            TestDate = dtoLabTest.TestDate,
                            Notes = dtoLabTest.Notes,
                            Report_Image = dtoLabTest.Report_Image,
                        };
                        await _db.LabTests.AddAsync(labTest);
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
