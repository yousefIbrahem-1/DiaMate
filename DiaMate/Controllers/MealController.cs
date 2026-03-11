using DiaMate.Data;
using DiaMate.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
