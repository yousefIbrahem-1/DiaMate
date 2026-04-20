using DiaMate.Data;
using DiaMate.Data.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiaMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        public FoodController(AppDbContext dp)
        {
            _db = dp;
        }

        private readonly AppDbContext _db;
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllFoodsName()
        {
          
                var foodNames = await _db.Foods
                                         .Select(f => f.Name)
                                         .Distinct()
                                         .ToListAsync();

            return Ok(foodNames);
        }

      
        [HttpGet("[action]")]
        public async Task<IActionResult> GetFoodByName(string name)
        {
            var food = await _db.Foods
                                     .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower());

            if (food == null)
                return NotFound("Food not found");
            dtoFood Food = new dtoFood()
            {
                Name = food.Name,
                CaloriesPer100g = food.CaloriesPer100g,
                Carbs_G = food.Carbs_G,
                Sugar_G = food.Sugar_G,
                Fiber_G = food.Fiber_G,
                Protein_G = food.Protein_G,
                Fat_G = food.Fat_G
            };
                

            return Ok(Food);
        }
    }
}
