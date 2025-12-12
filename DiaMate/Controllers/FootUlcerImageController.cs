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
    public class FootUlcerImageController : ControllerBase
    {
        public FootUlcerImageController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetAllFootUlcerImagesForPatient(int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var FootUlcerImages = await _db.FootUlcerImages.Where(F => F.PatientId == id).ToListAsync();
                    ICollection<dtoFootUlcerImage> dtoFootUlcerImages = new List<dtoFootUlcerImage>();
                    foreach (var f in FootUlcerImages)
                    {
                        dtoFootUlcerImages.Add(new dtoFootUlcerImage()
                        {
                            PatientId = f.PatientId,
                            Image = f.Image,
                            Ai_detectionResult = f.Ai_detectionResult,
                            AIConfidence = f.AIConfidence,
                            Notes = f.Notes,
                            UploadDate = f.UploadDate
                        });
                    }
                    return Ok(dtoFootUlcerImages);
                }
                else
                {
                    return NotFound("Patient is not exist");
                }
            }
            return Unauthorized();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetLastFootUlcerImageForPatient(int id)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var FootUlcerImage = await _db.FootUlcerImages
                                     .Where(f => f.PatientId == id)
                                     .OrderByDescending(f => f.UploadDate)
                                     .FirstOrDefaultAsync();


                    dtoFootUlcerImage dtoFootUlcerImage= new dtoFootUlcerImage()
                    {
                        PatientId = FootUlcerImage.PatientId,
                        Image = FootUlcerImage.Image,
                        Ai_detectionResult = FootUlcerImage.Ai_detectionResult,
                        AIConfidence = FootUlcerImage.AIConfidence,
                        Notes = FootUlcerImage.Notes,
                        UploadDate = FootUlcerImage.UploadDate
                    };
                    
                    return Ok(dtoFootUlcerImage);
                }
                else
                {
                    return NotFound("Patient is not exist");
                }
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetFootUlcerByDateForPatient(int id, DateTime UploadDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var images = await _db.FootUlcerImages
                                     .Where(f => (f.PatientId == id && (f.UploadDate.Year == UploadDate.Year &&
                                                                        f.UploadDate.Month == UploadDate.Month &&
                                                                        f.UploadDate.Day == UploadDate.Day)))
                                     .ToListAsync();

                    ICollection<dtoFootUlcerImage> dtoFootulcerImages = new List<dtoFootUlcerImage>();
                    foreach (var image in images)
                    {
                        dtoFootulcerImages.Add(new dtoFootUlcerImage
                        {
                            PatientId = image.PatientId,
                            Image = image.Image,
                            Ai_detectionResult = image.Ai_detectionResult,
                            AIConfidence = image.AIConfidence,
                            Notes = image.Notes,
                            UploadDate = image.UploadDate
                        });
                    }



                    return Ok(dtoFootulcerImages);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetFootUlcerByDateRangeForPatient(int id, DateTime fromUploadDate, DateTime toUploadDate)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == id)
            {
                var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (Patient != null)
                {
                    var images = await _db.FootUlcerImages
                                     .Where(f => (f.PatientId == id && (f.UploadDate>=fromUploadDate && f.UploadDate<=toUploadDate)))
                                     .ToListAsync();

                    ICollection<dtoFootUlcerImage> dtoFootulcerImages = new List<dtoFootUlcerImage>();
                    foreach (var image in images)
                    {
                        dtoFootulcerImages.Add(new dtoFootUlcerImage
                        {
                            PatientId = image.PatientId,
                            Image = image.Image,
                            Ai_detectionResult = image.Ai_detectionResult,
                            AIConfidence = image.AIConfidence,
                            Notes = image.Notes,
                            UploadDate = image.UploadDate
                        });
                    }



                    return Ok(dtoFootulcerImages);
                }
                return NotFound("this patient is not exist");
            }
            return Unauthorized();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddFootUlcerImageForPatient(dtoFootUlcerImage dtoFootUlcerImage)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == dtoFootUlcerImage.PatientId)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.FirstOrDefaultAsync((p => p.PatientId == dtoFootUlcerImage.PatientId));
                    if (Patient != null)
                    {
                        FootUlcerImage footUlcerImage = new FootUlcerImage()
                        {
                            PatientId = dtoFootUlcerImage.PatientId,
                            Image = dtoFootUlcerImage.Image,
                            Ai_detectionResult = dtoFootUlcerImage.Ai_detectionResult,
                            AIConfidence = dtoFootUlcerImage.AIConfidence,
                            Notes = dtoFootUlcerImage.Notes,
                            UploadDate = dtoFootUlcerImage.UploadDate

                        };
                        await _db.FootUlcerImages.AddAsync(footUlcerImage);
                        await _db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound("this Patient in not exist");
                    }

                }
                return BadRequest(ModelState);

            }

            return Unauthorized();
        }
    }
}
