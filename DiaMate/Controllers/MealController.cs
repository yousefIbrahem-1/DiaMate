using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.dtoModels;

//using DiaMate.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        public MealController(AppDbContext dp)
        {
            _db=dp;
        }

        private readonly AppDbContext _db;

        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetAllMealsForPatient(int PatientId)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Meals = await _db.Meals.Where(m => m.PatientId == PatientId).ToListAsync();
                    ICollection<dtoMeal> dtoMeals = new List<dtoMeal>();
                    foreach (var m in Meals)
                    {
                        dtoMeals.Add(new dtoMeal()
                        {
                            PatientId = m.PatientId,
                            Name = m.Name,
                            Read_date = m.Read_date,
                            Calories = m.Calories,
                            protein = m.protein,
                            carbs = m.carbs,
                            fats= m.fats,
                            Notes = m.Notes

                        });
                    }
                    return Ok(dtoMeals);
                }
                else
                {
                    return NotFound("this patient is not exist");
                }
            }
            return Unauthorized();
        }


        [HttpGet("[action]/{PatientId}")]
        public async Task<IActionResult> GetLastMealForPatient(int PatientId)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == PatientId)
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == PatientId);

                if (patient != null)
                {
                    var Meal = await _db.Meals.Where(m => m.PatientId == PatientId).OrderByDescending(m => m.Read_date).FirstOrDefaultAsync();


                    dtoMeal dtoMeal = new dtoMeal()
                    {
                        PatientId = Meal.PatientId,
                        Name = Meal.Name,
                        Read_date = Meal.Read_date,
                        Calories = Meal.Calories,
                        protein = Meal.protein,
                        carbs = Meal.carbs,
                        fats = Meal.fats,
                        Notes = Meal.Notes

                    };

                    return Ok(dtoMeal);
                
                }
                else
                {
                    return NotFound("this patient is not exist");
                }
            }
            return Unauthorized();
        }

        public async Task<IActionResult> AddNewMedicine(dtoMeal dtoMeal)
        {
            if (int.Parse(User.FindFirst("PatientId")?.Value) == dtoMeal.PatientId)
            {
                if (ModelState.IsValid)
                {
                    var Patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == dtoMeal.PatientId);
                    if (Patient != null)
                    {
                        Meal meal = new Meal()
                        {
                            PatientId = dtoMeal.PatientId,
                            Name = dtoMeal.Name,
                            Calories = dtoMeal.Calories,
                            protein  = dtoMeal.protein,
                            carbs = dtoMeal.carbs,
                            fats = dtoMeal.fats,
                            Read_date = dtoMeal.Read_date,
                            Notes = dtoMeal.Notes
                        };
                        await _db.Meals.AddAsync(meal);
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
