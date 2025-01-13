using EMS_Project.Models;
using EMS_Project.Repository.Holiday;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Security.Claims;

namespace EMS_Project.Controllers
{
    //[Authorize]
    public class HolidayController : Controller
    {
   
        private readonly ILogger<HolidayController> _logger;
        private readonly IHolidayRepository _IholidayRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string key = "HolidayKey";

        //Injecting Services to controller
        public HolidayController(ILogger<HolidayController> logger, IHolidayRepository holidayRepository, IMemoryCache memoryCache)
        {
            _logger = logger;
            _IholidayRepository = holidayRepository;
            _memoryCache = memoryCache;
        }


        //Add Holiday Get Method
        
        [HttpGet]
        public IActionResult AddHoliday()
        {
            string Id = HttpContext.User.FindFirstValue("id");
            string Email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            string UserName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return View();
        }


        //Add Holiday Post Method
        [HttpPost]
        public async Task<IActionResult> AddHoliday([FromBody]Holiday holiday)
        {

            try
            {
                await _IholidayRepository.AddHoliday(holiday);
                //return RedirectToAction("AddHoliday");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Holiday");
                return BadRequest(ex);
            }
           // return View(holiday);
           

        }


        //Get Method for All Holiday Details
        [HttpGet]
        public async Task<IActionResult> HolidayDetails()
        {
            //Data fetching message from cache or DB
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Holiday> Holidays;
            if (_memoryCache.TryGetValue(key, out Holidays))
            {
                _logger.Log(LogLevel.Information, "Data fetched from cache");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Data not fetched from cache");
                Holidays = await _IholidayRepository.GetAllHolidays();
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                    SlidingExpiration = TimeSpan.FromSeconds(45),
                    Priority = CacheItemPriority.Normal
                };
                _memoryCache.Set(key, Holidays, cacheEntryOptions);
            }
            stopwatch.Stop();
            _logger.Log(LogLevel.Information, "Time taken to fetch data: " + stopwatch.ElapsedMilliseconds);

            //return View(Holidays);
            return Ok(Holidays);
        }

        //Cache Clear Method
        public IActionResult ClearCache()
        {
            _memoryCache.Remove(key);
            _logger.Log(LogLevel.Information, "Cleared cache");
            return RedirectToAction("HolidayDetails");

        }

        //Delete Holiday by ID
        [HttpDelete]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var result = await _IholidayRepository.DeleteHoliday(id);
            if (result == false)
            {
                return NotFound();
            }
            //return RedirectToAction("HolidayDetails");
            return Ok();
        }


        //Edit Holiday Get Method
        [HttpGet]
        public async Task<IActionResult> EditHoliday(int id)
        {
            var holiday = await _IholidayRepository.GetHolidayById(id);
            if (holiday == null)
            {
                return NotFound();
            }
            //return View(holiday);
            return Ok(holiday);
        }


        //Edit Holiday Post Method[HttpPost]

        [HttpPut]
        public async Task<IActionResult> EditHoliday([FromBody]Holiday holiday,int id)
        {
            try
            {
                await _IholidayRepository.UpdateHoliday(holiday);
                //return RedirectToAction("HolidayDetails");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Holiday");
                return BadRequest(ex);
            }
            //return View(holiday);
            
        }
    }
}
